using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using Shop.Application.Exceptions;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.Validators.OrderValidations.AddOrderValidations;
using Shop.Domain.Entities;
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



                return Configuration.config.GetSection("SupportedLanguage:Launguages").Get<string[]>();


            }
        }

        private string DefaultLaunguage
        {
            get
            {
                return Configuration.config.GetSection("SupportedLanguage:Default").Get<string>();
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
        return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
             AddOrderDTOValidation validationRules = new AddOrderDTOValidation(LangCode);
            var validationResult = validationRules.Validate(addOrderDTO);

            if (!validationResult.IsValid)
                return new ErrorResult(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);


            User? user=await _userManager.FindByIdAsync(addOrderDTO.UserID.ToString());
            if (user is null)
                return new ErrorResult(message: HttpStatusErrorMessages.Unauthorized[LangCode], HttpStatusCode.NotFound);
            ShippingMethod? shippingMethod = _context.ShippingMethods.AsNoTracking().Where(x=>x.Id==addOrderDTO.ShippingMethod.Id).FirstOrDefault();
            if (shippingMethod is null)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            PaymentMethod? paymentMethod = _context.PaymentMethods.AsNoTracking().Where(x=>x.Id==addOrderDTO.PaymentMethod.Id).FirstOrDefault();
            if (paymentMethod is null)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
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
                




            };
            _context.Orders.Add(order);
           
            foreach (var product in addOrderDTO.Products)
            {
                Product? checkProduct = await _context.Products.AsNoTracking().Where(x=>x.Id==product.ProductId)
                    .Include(x=>x.ProductLanguages)
                    .Include(x=>x.SizeProducts)
                    .ThenInclude(y=>y.Size)
                    .FirstOrDefaultAsync();

                if (checkProduct is null)
                    return new ErrorResult(message: HttpStatusErrorMessages.NotFoundProduct[LangCode], HttpStatusCode.NotFound);


                if ((checkProduct.DisCount!=0 &&checkProduct.DisCount!=product.Price)||checkProduct.Price!=product.Price)

                    return new ErrorResult(message: HttpStatusErrorMessages.PriceNotMatch[LangCode], HttpStatusCode.BadRequest);

                var checkSize=checkProduct.SizeProducts?.FirstOrDefault(x => x.Size.Content == product.size);
                if (checkSize is null)
                    return new ErrorResult(message: HttpStatusErrorMessages.SizeNotFound[LangCode], HttpStatusCode.NotFound);
                if (checkSize.StockQuantity<product.Quantity)
                    return new ErrorResult(message: HttpStatusErrorMessages.StockQuantityNotEnough[LangCode], HttpStatusCode.BadRequest);


                SoldProduct soldProduct = new()
                {

                    Quantity = product.Quantity,
                    SoldPrice = checkProduct.DisCount != 0 ? checkProduct.DisCount : checkProduct.Price,
                    SoldTime = DateTime.UtcNow,
                    ProductCode = checkProduct.ProductCode,
                    ProductName = checkProduct.ProductLanguages.FirstOrDefault(x => x.LanguageCode == LangCode)?.Title ?? checkProduct.ProductLanguages.FirstOrDefault().Title,
                    ProductId = product.ProductId,
                    SizeId = checkSize.SizeId,
                    OrderId = order.Id,


                };
                _context.SoldProducts.Add(soldProduct);


            }


           IDataResult<List<string>> file= _fileService.SaveOrderPdf(addOrderDTO.Products,addOrderDTO.ShippingMethod,addOrderDTO.PaymentMethod,new OrderUserInfoDTO
            {
                Address = addOrderDTO.Address,
                FullName = addOrderDTO.FullName,
                PhoneNumber = addOrderDTO.PhoneNumber,
               Note=addOrderDTO.Note,            

            });
            if (file.IsSuccess)
            {
                
            IResult mailResul  = await _mailService.SendEmailPdfAsync(user.Email, user.FirstName + " " + user.LastName, file.Data[0]);
                if (mailResul.IsSuccess) {
                    order.OrderNumber = file.Data[1];
                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    _logger.LogError("OrderService.AddOrderAsync: Email sending failed: ", mailResul.Message);

                    return new ErrorResult(message: mailResul.Message, statusCode: mailResul.StatusCode);
                }
            }

         return new ErrorResult(message: file.Message, statusCode: file.StatusCode);
        }
    }
}
