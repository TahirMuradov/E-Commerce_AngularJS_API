
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Domain.Exceptions;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Application.Validators.ProductValidations;
using Shop.Domain.Entities;
using Shop.Persistence.Context;
using System.Net;

namespace Shop.Persistence.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDBContext _context;
        private readonly ILogger<ProductService> _logger;
        private readonly IFileService _fileService;
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
        public ProductService(AppDBContext context, ILogger<ProductService> logger, IFileService fileService)
        {
            _context = context;
            _logger = logger;
            _fileService = fileService;
        }

        public async Task<IResult> AddProductAsync(AddProductDTO addProductDTO, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.BadRequest);
            AddProductDTOValidation validationRules = new AddProductDTOValidation(LangCode, SupportedLaunguages);
            var validationResult = validationRules.Validate(addProductDTO);
            if (validationResult.IsValid)
                return new ErrorResult(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            Category? category = _context.Categories.AsNoTracking().FirstOrDefault(x => x.Id == addProductDTO.CategoryId);
            if (category is null)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);

            Product product = new Product()
            {
                CategoryId = category.Id,
                IsFeatured = addProductDTO.Isfeature,
                Price = addProductDTO.Price,
                DisCount = addProductDTO.Discount,
                ProductCode = addProductDTO.ProductCode,
                ImageUrls = new List<string>(),

            };
            _context.Products.Add(product);
            var existingSizeIds = _context.Sizes
         .AsNoTracking()
         .Select(s => s.Id)
         .ToHashSet();
            bool allSizesExist = addProductDTO.Sizes.Keys.All(id => existingSizeIds.Contains(id));
            if (allSizesExist)
            {

                foreach (var size in addProductDTO.Sizes)
                {

                    SizeProduct sizeProduct = new SizeProduct()
                    {
                        ProductId = product.Id,
                        SizeId = size.Key,
                        StockQuantity = size.Value
                    };
                    _context.SizeProducts.Add(sizeProduct);
                }
            }
            else
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], statusCode: HttpStatusCode.NotFound);

            foreach (Microsoft.AspNetCore.Http.IFormFile image in addProductDTO.ProductImages)
            {
                if (image.Length > 0)
                {
                    IDataResult<string> imageUrl = await _fileService.SaveFileAsync(image, true);
                    if (!imageUrl.IsSuccess)
                        return new ErrorResult(message: HttpStatusErrorMessages.FileUploadFailed[LangCode], HttpStatusCode.InternalServerError);
                    product.ImageUrls.Add(imageUrl.Data);

                }

            }
            _context.Products.Update(product);
            try
            {
                await _context.SaveChangesAsync();
                return new SuccessResult(message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }




        }

        public IResult DeleteProduct(Guid id, string LangCode)
        {
            if (id == Guid.Empty)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.BadRequest);
            Product product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product is null)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            _fileService.RemoveFileRange(product.ImageUrls);
            _context.Products.Remove(product);
            try
            {
                _context.SaveChanges();
                return new SuccessResult(message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
        }

        public async Task<IDataResult<PaginatedList<GetProductDTO>>> GetAllProductByPageOrSearchAsync(int page, string LangCode, string? search = null)
        {
            IQueryable<GetProductDTO> productQuery = search is null? _context.Products.AsNoTracking().AsSplitQuery().Select(x => new GetProductDTO
            {
                Id = x.Id,
                ProductCode = x.ProductCode,
                Price = x.Price,
                Discount = x.DisCount,
                Title = x.ProductLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Title,
                CategoryName = x.Category.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Name,
                ImageUrl = x.ImageUrls.FirstOrDefault(),




            }): _context.Products.AsNoTracking().AsSplitQuery().Select(x => new GetProductDTO
            {
                Id = x.Id,
                ProductCode = x.ProductCode,
                Price = x.Price,
                Discount = x.DisCount,
                Title = x.ProductLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Title,
                CategoryName = x.Category.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Name,
                ImageUrl = x.ImageUrls.FirstOrDefault(),




            }).Where(x=>x.Title.Contains(search, StringComparison.InvariantCultureIgnoreCase) ||
            x.CategoryName.Contains(search, StringComparison.InvariantCultureIgnoreCase) ||
            x.ProductCode.Contains(search,StringComparison.InvariantCultureIgnoreCase) ||
            x.Discount.ToString().Contains(search, StringComparison.InvariantCultureIgnoreCase) ||
            x.Id.ToString().Contains(search, StringComparison.InvariantCultureIgnoreCase)
            );
            PaginatedList<GetProductDTO> paginatedProducts = await PaginatedList<GetProductDTO>.CreateAsync(productQuery, page, 10);
            return new SuccessDataResult<PaginatedList<GetProductDTO>>(paginatedProducts, message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
        }

        public IDataResult<IQueryable<GetProductDTO>> GetProductByFeatured(string LangCode)
        {
            try
            {
                IQueryable<GetProductDTO>? productQuery = _context.Products.AsNoTracking().Where(x => x.IsFeatured)?.Select(x => new GetProductDTO
                {
                    Id = x.Id,
                    ProductCode = x.ProductCode,
                    Price = x.Price,
                    Discount = x.DisCount,
                    Title = x.ProductLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Title,
                    CategoryName = x.Category.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Name,
                    ImageUrl = x.ImageUrls.FirstOrDefault(),





                });
               
                return new SuccessDataResult<IQueryable<GetProductDTO>>(productQuery, message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorDataResult<IQueryable<GetProductDTO>>(message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);


            }
     
        }

        public IDataResult<GetProductDetailDTO> GetProductById(Guid id, string LangCode)
        {
            try
            {
                GetProductDetailDTO productQuery = _context.Products
   .AsNoTracking()
   .Where(x => x.Id == id)
   .Select(x => new GetProductDetailDTO
   {
       Id = x.Id,
       CategoryName = x.Category.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Name,
       Discount = x.DisCount,
       Title = x.ProductLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Title,
       ImageUrls = x.ImageUrls,
       Description = x.ProductLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Description,
       Price = x.Price,
       ProductCode = x.ProductCode,
       Sizes = x.SizeProducts.Select(sp => new GetSizeForProductDTO
       {
           Id = sp.Size.Id,
           Size = sp.Size.Content,
           StockCount = sp.StockQuantity
       }).ToList(),
       RelatedProducts = _context.Products
           .AsNoTracking()
           .Where(p => p.CategoryId == x.CategoryId && p.Id != x.Id)
           .Select(p => new GetProductDTO
           {
               Id = p.Id,
               ProductCode = p.ProductCode,
               Price = p.Price,
               Discount = p.DisCount,
               Title = p.ProductLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Title,
               CategoryName = p.Category.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Name,
               ImageUrl = p.ImageUrls.FirstOrDefault()
           }).ToList()
   })
   .FirstOrDefault();

                return productQuery is null ?
                  new ErrorDataResult<GetProductDetailDTO>(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound) :
                    new SuccessDataResult<GetProductDetailDTO>(data: productQuery, HttpStatusCode.OK);


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorDataResult<GetProductDetailDTO>(message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
          
        }
        public async Task<IResult> UpdateProductAsync(UpdateProductDTO updateProductDTO, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.BadRequest);

            var validationRules = new UpdateProductDTOValidation(LangCode, SupportedLaunguages);
            var validationResult = validationRules.Validate(updateProductDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);

            // Find product to update
            var product = await _context.Products
                .Include(p => p.ProductLanguages)
                .Include(p => p.SizeProducts)
                .FirstOrDefaultAsync(p => p.Id == updateProductDTO.Id);

            if (product == null)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);

            // Update basic fields
            product.ProductCode = updateProductDTO.ProductCode;
            product.Price = updateProductDTO.Price;
            product.DisCount = updateProductDTO.Discount;

            // Update localized titles and descriptions
            foreach (var langKey in updateProductDTO.Title.Keys)
            {
                var productLang = product.ProductLanguages.FirstOrDefault(pl => pl.LanguageCode == langKey);
                if (productLang != null)
                {
                    productLang.Title = updateProductDTO.Title[langKey];
                    productLang.Description = updateProductDTO.Description.ContainsKey(langKey)
                        ? updateProductDTO.Description[langKey]
                        : productLang.Description;
                }
                else
                {
                    // Add new localization if it does not exist
                    product.ProductLanguages.Add(new ProductLanguage
                    {
                        ProductId = product.Id,
                        LanguageCode = langKey,
                        Title = updateProductDTO.Title[langKey],
                        Description = updateProductDTO.Description.ContainsKey(langKey) ? updateProductDTO.Description[langKey] : null
                    });
                }
            }

            // Update sizes and stock counts
            var existingSizeIds = _context.Sizes.Select(s => s.Id).ToHashSet();
            bool allSizesExist = updateProductDTO.Sizes.Keys.All(id => existingSizeIds.Contains(id));
            if (!allSizesExist)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], statusCode: HttpStatusCode.NotFound);

            // Update or add size products
            foreach (var sizeEntry in updateProductDTO.Sizes)
            {
                var sizeProduct = product.SizeProducts.FirstOrDefault(sp => sp.SizeId == sizeEntry.Key);
                if (sizeProduct != null)
                {
                    // Update stock count
                    sizeProduct.StockQuantity = sizeEntry.Value;
                }
                else
                {
                    // Add new SizeProduct relation
                    product.SizeProducts.Add(new SizeProduct
                    {
                        ProductId = product.Id,
                        SizeId = sizeEntry.Key,
                        StockQuantity = sizeEntry.Value
                    });
                }
            }

            // Delete images by URL if any
            if (updateProductDTO.DeletedImageUrls != null && updateProductDTO.DeletedImageUrls.Any())
            {
                foreach (var urlToDelete in updateProductDTO.DeletedImageUrls)
                {
                    if (product.ImageUrls.Contains(urlToDelete))
                    {
                        product.ImageUrls.Remove(urlToDelete);
                        // Optionally, delete file physically via file service if needed
                        _fileService.RemoveFile(urlToDelete);
                    }
                }
            }

            // Add new images
            if (updateProductDTO.NewImages != null && updateProductDTO.NewImages.Count > 0)
            {
                foreach (var newImage in updateProductDTO.NewImages)
                {
                    if (newImage.Length > 0)
                    {
                        var imageUrlResult = await _fileService.SaveFileAsync(newImage, true);
                        if (!imageUrlResult.IsSuccess)
                            return new ErrorResult(message: HttpStatusErrorMessages.FileUploadFailed[LangCode], HttpStatusCode.InternalServerError);

                        product.ImageUrls.Add(imageUrlResult.Data);
                    }
                }
            }

            _context.Products.Update(product);

            try
            {
                await _context.SaveChangesAsync();
                return new SuccessResult(message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
        }

    }
}
