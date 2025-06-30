using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.AuthDTOs;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using Shop.Application.DTOs.PaymentMethodDTOs;
using Shop.Application.DTOs.ShippingMethodDTOs;
using Shop.Application.DTOs.SoldProductDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Application.Validators.OrderValidations.AddOrderValidations;
using Shop.Domain.Entities;
using Shop.Domain.Enums;
using Shop.Domain.Exceptions;
using Shop.Persistence.Context;
using System.Net;

namespace Shop.Persistence.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDBContext _context;
        private readonly IFileService _fileService;
        private readonly IMailService _mailService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<OrderService> _logger;
        private string[] SupportedLaunguages
        {
            get
            {



                return Configuration.SupportedLaunguageKeys;


            }
        }

        private string DefaultLaunguage
        {
            get
            {
                return Configuration.DefaultLanguageKey;
            }
        }
        public OrderService(AppDBContext context, IFileService fileService, IMailService mailService, ILogger<OrderService> logger, UserManager<User> userManager)
        {
            _context = context;
            _fileService = fileService;
            _mailService = mailService;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<IResult> AddOrderAsync(AddOrderDTO addOrderDTO, string LangCode)
        {

            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            AddOrderDTOValidation validationRules = new AddOrderDTOValidation(LangCode);
            var validationResult = validationRules.Validate(addOrderDTO);

            if (!validationResult.IsValid)
                return new ErrorResult(LangCode,messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);


            User? user = await _userManager.FindByIdAsync(addOrderDTO.UserID.ToString());
            if (user is null)
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.Unauthorized[LangCode], HttpStatusCode.NotFound);
            ShippingMethod? shippingMethod = await _context.ShippingMethods.AsNoTracking().Where(x => x.Id == addOrderDTO.ShippingMethod.Id).FirstOrDefaultAsync();
            if (shippingMethod is null)
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            PaymentMethod? paymentMethod = await _context.PaymentMethods.AsNoTracking().Where(x => x.Id == addOrderDTO.PaymentMethod.Id).FirstOrDefaultAsync();
            if (paymentMethod is null)
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            Order order = new Order()
            {
                UserId = addOrderDTO.UserID,
                FullName = addOrderDTO.FullName,
                PhoneNumber = addOrderDTO.PhoneNumber,
                Address = addOrderDTO.Address,
                Note = addOrderDTO.Note,
                ShippingPrice = shippingMethod.Price,
                ShippingMethodId = shippingMethod.Id,
                PaymentMethodId = paymentMethod.Id,
                CreatedAt = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending

            };
            _context.Orders.Add(order);

            foreach (var product in addOrderDTO.Products)
            {
                Product? checkProduct = await _context.Products.AsNoTracking().Where(x => x.Id == product.Id)
                    .Include(x => x.ProductLanguages)
                    .Include(x => x.SizeProducts)
                    .ThenInclude(y => y.Size)
                    .FirstOrDefaultAsync();

                if (checkProduct is null)
                    return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFoundProduct[LangCode], HttpStatusCode.NotFound);


                if ((checkProduct.DisCount != 0 && checkProduct.DisCount != product.Price) || checkProduct.Price != product.Price)

                    return new ErrorResult(LangCode,message: HttpStatusErrorMessages.PriceNotMatch[LangCode], HttpStatusCode.BadRequest);

                var checkSize = checkProduct.SizeProducts?.FirstOrDefault(x => x.Size.Content == product.size);
                if (checkSize is null)
                    return new ErrorResult(LangCode,message: HttpStatusErrorMessages.SizeNotFound[LangCode], HttpStatusCode.NotFound);
                if (checkSize.StockQuantity < product.Quantity)
                    return new ErrorResult(LangCode,message: HttpStatusErrorMessages.StockQuantityNotEnough[LangCode], HttpStatusCode.BadRequest);


                SoldProduct soldProduct = new()
                {

                    Quantity = product.Quantity,
                    SoldPrice = checkProduct.DisCount != 0 ? checkProduct.DisCount : checkProduct.Price,
                    SoldTime = DateTime.UtcNow,
                    ProductCode = checkProduct.ProductCode,
                    ProductName = checkProduct.ProductLanguages.FirstOrDefault(x => x.LanguageCode == LangCode)?.Title ?? checkProduct.ProductLanguages.FirstOrDefault().Title,
                    ProductId = product.Id,
                    SizeId = checkSize.SizeId,
                    OrderId = order.Id,


                };
                _context.SoldProducts.Add(soldProduct);


            }


            IDataResult<List<string>> file = _fileService.SaveOrderPdf(LangCode,addOrderDTO.Products, addOrderDTO.ShippingMethod, addOrderDTO.PaymentMethod, new OrderUserInfoDTO
            {
                Address = addOrderDTO.Address,
                FullName = addOrderDTO.FullName,
                PhoneNumber = addOrderDTO.PhoneNumber,
                Note = addOrderDTO.Note,

            });
            if (file.IsSuccess)
            {

                IResult mailResul = await _mailService.SendEmailPdfAsync(LangCode,user.Email, user.FirstName + " " + user.LastName, file.Data[0]);
                if (mailResul.IsSuccess)
                {
                    order.OrderNumber = file.Data[1];
                    order.OrderPdfPath = file.Data[0];

                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    _logger.LogError("OrderService.AddOrderAsync: Email sending failed: ", mailResul.Message);

                    return new ErrorResult(LangCode,message: mailResul.Message, statusCode: mailResul.StatusCode);
                }
            }

            return new ErrorResult(LangCode,message: file.Message, statusCode: file.StatusCode);
        }

        public async Task<IDataResult<GetOrderDetailDTO>> GetOrderByIdAsync(Guid orderId, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<GetOrderDetailDTO>(LangCode,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            if (orderId == Guid.Empty)
                return new ErrorDataResult<GetOrderDetailDTO>(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);


            try
            {
                GetOrderDetailDTO? order = await _context.Orders.AsNoTracking().Where(x => x.Id == orderId).Select(x => new GetOrderDetailDTO
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    Note = x.Note,
                    ShippingMethod = new GetShippingMethodDTO
                    {
                        Id = x.ShippingMethod.Id,
                        content = x.ShippingMethod.ShippingMethodLanguages.Where(lg => lg.LangCode == LangCode).Select(cn => cn.Content).FirstOrDefault(),
                        Price = x.ShippingMethod.Price,
                        DisCount = x.ShippingMethod.DisCountPrice

                    },
                    PaymentMethod = new GetPaymentMethodDTO
                    {
                        Id = x.PaymentMethod.Id,
                        Content = x.PaymentMethod.PaymentMethodLanguages.Where(lg => lg.LangCode == LangCode).Select(cn => cn.Content).FirstOrDefault(),
                        IsCash = x.PaymentMethod.IsCash
                    },
                    CreatedDate = x.CreatedAt,
                    OrderNumber = x.OrderNumber,
                    OrderPdfPath = x.OrderPdfPath,
                    Status = x.OrderStatus,
                    TotalPrice = x.SoldProducts.Sum(sp => sp.SoldPrice * sp.Quantity),
                    OrderBy = new GetUserDTO
                    {
                        Adress = x.User.Adress,
                        Email = x.User.Email,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        PhoneNumber = x.User.PhoneNumber,
                        Id = x.User.Id,
                        UserName = x.User.UserName,

                    },
                    SoldsProducts = x.SoldProducts.Select(sp => new GetSoldProductDTO
                    {
                        Id = sp.Id,
                        ProductId = sp.Id,
                        ProductName = sp.ProductName,
                        ProductCode = sp.ProductCode,
                        SoldPrice = sp.SoldPrice,
                        Quantity = sp.Quantity,
                        Size = sp.Size.Content,
                        OrderId = sp.OrderId,
                        SoldTime = sp.SoldTime,
                        OrderNumber = x.OrderNumber,
                        OrderPath = x.OrderPdfPath,

                    }).ToList()

                }).FirstOrDefaultAsync();
                if (order is null)
                    return new ErrorDataResult<GetOrderDetailDTO>(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);

                User? Orderby = await _userManager.FindByIdAsync(order.OrderBy.Id.ToString());
                if (Orderby is null)
                    return new ErrorDataResult<GetOrderDetailDTO>(LangCode,message: HttpStatusErrorMessages.Unauthorized[LangCode], HttpStatusCode.NotFound);

                var roles = await _userManager.GetRolesAsync(Orderby);
                order.OrderBy.Roles = roles.ToList();
                return new SuccessDataResult<GetOrderDetailDTO>(order,LangCode, message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorDataResult<GetOrderDetailDTO>(LangCode,message: ex.Message, statusCode: HttpStatusCode.InternalServerError);

            }

        }



        public async Task<IResult> UpdateOrderStatusAsync(Guid orderId, OrderStatus status, string LangCode)
        {
            if (orderId == Guid.Empty)

                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            Order? order = await _context.Orders.Where(x => x.Id == orderId).FirstOrDefaultAsync();
            if (order is null)
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            try
            {
                order.OrderStatus = status;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return new SuccessResult(LangCode,message: HttpStatusErrorMessages.Success[LangCode], statusCode: HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(LangCode,message: ex.Message, statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<IResult> UpdateOrderAsync(UpdateOrderDTO updateOrderDTO, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            UpdateOrderDTOValidation validationRules = new UpdateOrderDTOValidation(LangCode);
            var validationResult = validationRules.Validate(updateOrderDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(LangCode,messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);


            try
            {
                Order? order = _context.Orders
            .Include(x => x.SoldProducts)
            .Include(x => x.ShippingMethod)
            .Include(x => x.PaymentMethod)
            .FirstOrDefault(x => x.Id == updateOrderDTO.OrderId);
                if (order is null)
                    return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);

                OrderShippingMethodDTO? shippingMethod = await _context.ShippingMethods.AsNoTracking().Where(x => x.Id == updateOrderDTO.ShippingMethod.Id)
                    .Select(x => new OrderShippingMethodDTO
                    {
                        Price = x.DisCountPrice > 0 ? x.DisCountPrice : x.Price,
                        Id = x.Id,
                        ShippingContent = x.ShippingMethodLanguages.Where(lg => lg.LangCode == LangCode).Select(cn => cn.Content).FirstOrDefault(),

                    }).FirstOrDefaultAsync();

                if (shippingMethod == default || shippingMethod.Id == Guid.Empty || shippingMethod is null)
                    return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);


                order.ShippingMethodId = shippingMethod.Id;
                order.FullName = updateOrderDTO.FullName;
                order.PhoneNumber = updateOrderDTO.PhoneNumber;
                order.Address = updateOrderDTO.Address;
                order.Note = updateOrderDTO.Note;
                order.OrderStatus = OrderStatus.UpdateCustomerInfo;
                order.ShippingPrice = updateOrderDTO.ShippingMethod.Price;

                _context.Orders.Update(order);


              var removeOldOrderResult=  _fileService.RemoveFile(LangCode,order.OrderPdfPath);

                if (removeOldOrderResult.IsSuccess)
                {
                    List<OrderProductDTO> orderProductDTOs=await _context.Orders.Where(x => x.Id == updateOrderDTO.OrderId)
                        .SelectMany(x => x.SoldProducts.Select(sp => new OrderProductDTO
                        {
                                Id = sp.ProductId,
                            Price = sp.SoldPrice,
                            Quantity = sp.Quantity,
                            size = sp.Size.Content,
                            ProductCode = sp.ProductCode,
                            ProductName = sp.ProductName
                        })).ToListAsync();





                    var fileResult = _fileService.SaveOrderPdf(LangCode,orderProductDTOs, new OrderShippingMethodDTO
                    {
                        Id = shippingMethod.Id,
                        ShippingContent = shippingMethod.ShippingContent,
                        Price = shippingMethod.Price,
                    }, new OrderPaymentMethodDTO
                    {
                        Id = order.PaymentMethodId,
                        Content = order.PaymentMethod.PaymentMethodLanguages.FirstOrDefault(x => x.LangCode == LangCode).Content,

                    }, new OrderUserInfoDTO
                    {
                        Address = order.Address,
                        FullName = order.FullName,
                        PhoneNumber = order.PhoneNumber,
                        Note = order.Note,


                    });
                    if (!fileResult.IsSuccess)
                        return new ErrorResult(LangCode,message: fileResult.Message, statusCode: fileResult.StatusCode);
                    order.OrderPdfPath = fileResult.Data[0];
                    order.OrderNumber = fileResult.Data[1];
                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();
                return new SuccessResult(LangCode,message: HttpStatusErrorMessages.Success[LangCode], statusCode: HttpStatusCode.OK);
                    
                }
                else
                    return new ErrorResult(LangCode,message: removeOldOrderResult.Message, statusCode: removeOldOrderResult.StatusCode);
            }
            catch (Exception ex)
            {


                _logger.LogError(ex, ex.Message);
                return new ErrorResult(LangCode,message: ex.Message, statusCode: HttpStatusCode.InternalServerError);
            }

        



        }

        public async Task<IDataResult<PaginatedList<GetOrderDTO>>> GetAllOrdersByPageOrSearchAsync(int page, string LangCode, string? search = null)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                    return new ErrorDataResult<PaginatedList<GetOrderDTO>>(LangCode,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);

            if (page < 1)
                page = 1;

            try
            {

           IQueryable<GetOrderDTO> queryOrder = search is null?  _context.Orders.AsNoTracking().AsSplitQuery().Select(x => new GetOrderDTO
           {
                Id = x.Id,
                FullName = x.FullName,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Note = x.Note,
                CreatedDate = x.CreatedAt,
                OrderNumber = x.OrderNumber,
                OrderPdfPath = x.OrderPdfPath,
                Status = x.OrderStatus,
                TotalPrice = x.SoldProducts.Sum(sp => sp.SoldPrice * sp.Quantity),
                OrderBy = new GetUserDTO
                {
                    Adress = x.User.Adress,
                    Email = x.User.Email,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    PhoneNumber = x.User.PhoneNumber,
                    Id = x.User.Id,
                    UserName = x.User.UserName,

                }
               

            }):
            _context.Orders.AsNoTracking().AsSplitQuery().Select(x => new GetOrderDTO
            {
                Id = x.Id,
                FullName = x.FullName,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Note = x.Note,
                CreatedDate = x.CreatedAt,
                OrderNumber = x.OrderNumber,
                OrderPdfPath = x.OrderPdfPath,
                Status = x.OrderStatus,
                TotalPrice = x.SoldProducts.Sum(sp => sp.SoldPrice * sp.Quantity),
                OrderBy = new GetUserDTO
                {
                    Adress = x.User.Adress,
                    Email = x.User.Email,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    PhoneNumber = x.User.PhoneNumber,
                    Id = x.User.Id,
                    UserName = x.User.UserName,

                }


            }).Where(x=>x.FullName.ToLower().Contains(search.ToLower()) || 
            x.OrderNumber.ToLower().Contains(search.ToLower())||
            x.OrderBy.UserName.ToLower().Contains(search.ToLower()) ||
            x.Address.ToLower().Contains(search.ToLower())||
            x.PhoneNumber.ToLower().Contains(search.ToLower())||
            x.Note.ToLower().Contains(search.ToLower())||
            x.CreatedDate.ToString().Contains(search.ToLower())||
            x.Status.ToString().ToLower().Contains(search.ToLower())||
            x.TotalPrice.ToString().Contains(search.ToLower())
            );
                var paginatedData = await PaginatedList<GetOrderDTO>.CreateAsync(queryOrder, page, 10);
                return new SuccessDataResult<PaginatedList<GetOrderDTO>>(data: paginatedData,LangCode, message: HttpStatusErrorMessages.Success[LangCode], statusCode: HttpStatusCode.OK);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorDataResult<PaginatedList<GetOrderDTO>>(LangCode,message: ex.Message, statusCode: HttpStatusCode.InternalServerError);


            }


          
        }

        public async Task<IResult> DeleteOrderAsync(Guid orderId, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<GetOrderDetailDTO>(LangCode,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            if (orderId == Guid.Empty)
                return new ErrorDataResult<GetOrderDetailDTO>(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            Order? order = await _context.Orders.Include(x=>x.SoldProducts).Where(x => x.Id == orderId).FirstOrDefaultAsync();
            if (order is null)
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            try
            {
                _context.Orders.Remove(order);
                _context.SoldProducts.RemoveRange(order.SoldProducts);
               await _context.SaveChangesAsync();
                return new SuccessResult(LangCode,message: HttpStatusErrorMessages.Success[LangCode], statusCode: HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(LangCode,message: ex.Message, statusCode: HttpStatusCode.InternalServerError);
            }

        }

        public async Task<IDataResult<PaginatedList<GetOrderByUserDTO>>> GetAllOrdersByUserIdAsync(string userId, int page, string LangCode)
        {
          
                if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                    return new ErrorDataResult<PaginatedList<GetOrderByUserDTO>>(LangCode,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            if (string.IsNullOrEmpty(userId))
                return new ErrorDataResult<PaginatedList<GetOrderByUserDTO>>(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);

            if (page < 1)
                page = 1;


            try
            {
                User? user=await _userManager.FindByIdAsync(userId);
                if (user is null)
                    return new ErrorDataResult<PaginatedList<GetOrderByUserDTO>>(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);

                IQueryable<GetOrderByUserDTO> queryOrder = _context.Orders.AsNoTracking().Where(x=>x.UserId==user.Id).Select(x => new GetOrderByUserDTO
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    Note = x.Note,
                    CreatedDate = x.CreatedAt,
                    OrderNumber = x.OrderNumber,
                    OrderPdfPath = x.OrderPdfPath,
                    Status = x.OrderStatus,
                    TotalPrice = x.SoldProducts.Sum(sp => sp.SoldPrice * sp.Quantity),
                           });
                var paginatedData = await PaginatedList<GetOrderByUserDTO>.CreateAsync(queryOrder, page, 10);
                return new SuccessDataResult<PaginatedList<GetOrderByUserDTO>>(data: paginatedData,LangCode, message: HttpStatusErrorMessages.Success[LangCode], statusCode: HttpStatusCode.OK);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorDataResult<PaginatedList<GetOrderByUserDTO>>(LangCode,message: ex.Message, statusCode: HttpStatusCode.InternalServerError);


            }
        }
    }



}
