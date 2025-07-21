
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



                return ConfigurationPersistence.SupportedLaunguageKeys;


            }
        }

        private string DefaultLaunguage
        {
            get
            {
                return ConfigurationPersistence.DefaultLanguageKey;
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
                return new ErrorResult(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.BadRequest);
            AddProductDTOValidation validationRules = new AddProductDTOValidation(LangCode, SupportedLaunguages);
            var validationResult = validationRules.Validate(addProductDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(LangCode,messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            Category? category = _context.Categories.AsNoTracking().FirstOrDefault(x => x.Id == addProductDTO.CategoryId);
            if (category is null)
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);

            Product product = new Product()
            {
                CategoryId = category.Id,
                IsFeatured = addProductDTO.Isfeature,
                Price = addProductDTO.Price,
                DisCount = addProductDTO.Discount,
                ProductCode = addProductDTO.ProductCode,
               

            };
            _context.Products.Add(product);
          await  _context.SaveChangesAsync();
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
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], statusCode: HttpStatusCode.NotFound);

            foreach (Microsoft.AspNetCore.Http.IFormFile image in addProductDTO.ProductImages)
            {
                if (image.Length > 0)
                {
                    IDataResult<string> imageUrl = await _fileService.SaveImageAsync(LangCode,image, true);
                    if (!imageUrl.IsSuccess)
                        return new ErrorResult(LangCode,message: HttpStatusErrorMessages.FileUploadFailed[LangCode], HttpStatusCode.InternalServerError);
                    

                    Image imageEntity = new Image()
                    {
                        Path = imageUrl.Data,
                        ProductId = product.Id
                    };
              
                    _context.Images.Add(imageEntity);

                }

            }
            foreach (var content in addProductDTO.Title)
            {
                ProductLanguage productLanguage = new()
                {
                    ProductId = product.Id,
                    Description = addProductDTO.Description[content.Key],
                    LanguageCode = content.Key,
                    Title = content.Value
                };
                _context.ProductLanguages.Add(productLanguage);

                
            }
        
            try
            {
                await _context.SaveChangesAsync();
                return new SuccessResult(LangCode,message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }




        }

        public IResult DeleteProduct(Guid id, string LangCode)
        {
            if (id == Guid.Empty)
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            Product? product = _context.Products.Include(x=>x.Images).FirstOrDefault(x => x.Id == id);
            if (product is null)
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
           IResult removingResult=  _fileService.RemoveFileRange(LangCode,product.Images.Select(x=>x.Path).ToList());
            if (removingResult.IsSuccess)          
            _context.Products.Remove(product);
            else
                return new ErrorResult(LangCode,message: removingResult.Message, removingResult.StatusCode);
            try
            {
                _context.SaveChanges();
                return new SuccessResult(LangCode,message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
        }

        public async Task<IDataResult<PaginatedList<GetProductDTO>>> GetAllProductByPageOrSearchAsync(int page, string LangCode, string? search = null)
        {
            IQueryable<GetProductDTO> productQuery = search is null? _context.Products.AsNoTracking().Select(x => new GetProductDTO
            {
                Id = x.Id,
                ProductCode = x.ProductCode,
                Price = x.Price,
                Discount = x.DisCount,
                Title = x.ProductLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Title,
                CategoryName = x.Category.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Name,
                ImageUrl = x.Images[0].Path,
                SizeInfo=x.SizeProducts.Select(sp => new GetSizeForProduct
                {
                 
                    Size = sp.Size.Content,
                    StockCount = sp.StockQuantity
                }).ToList()



            }): _context.Products.AsNoTracking().Select(x => new GetProductDTO
            {
                Id = x.Id,
                ProductCode = x.ProductCode,
                Price = x.Price,
                Discount = x.DisCount,
                Title = x.ProductLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Title,
                CategoryName = x.Category.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Name,
                ImageUrl = x.Images[0].Path,
                SizeInfo = x.SizeProducts.Select(sp => new GetSizeForProduct
                {
                  
                    Size = sp.Size.Content,
                    StockCount = sp.StockQuantity
                }).ToList()



            }).Where(x=>x.Title.ToLower().Contains(search.ToLower())||
            x.CategoryName.ToLower().Contains(search.ToLower()) ||
            x.ProductCode.ToLower().Contains(search.ToLower())||
            x.Discount.ToString().Contains(search)||
            x.Id.ToString().ToLower().Contains(search.ToLower())
            );
            PaginatedList<GetProductDTO> paginatedProducts = await PaginatedList<GetProductDTO>.CreateAsync(productQuery, page, 10);
            return new SuccessDataResult<PaginatedList<GetProductDTO>>(paginatedProducts, LangCode,message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
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
                    ImageUrl = x.Images[0].Path,





                });
               
                return new SuccessDataResult<IQueryable<GetProductDTO>>(productQuery, LangCode,message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorDataResult<IQueryable<GetProductDTO>>(LangCode,message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);


            }
     
        }

        public async Task<IDataResult<GetProductDetailDTO>> GetProductByIdAsync(Guid id, string LangCode)
        {
            try
            {
                GetProductDetailDTO ?productQuery =await _context.Products
   .AsNoTracking()

   .Select(x => new GetProductDetailDTO
   {
       Id = x.Id,
       CategoryName = x.Category.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Name,
       Discount = x.DisCount,
       Title = x.ProductLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Title,
       ImageUrls = x.Images.Select(sl => sl.Path).ToList(),
       Description = x.ProductLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Description,
       Price = x.Price,
       ProductCode = x.ProductCode,
       Sizes = x.SizeProducts.Select(sp => new GetSizeForProductDetailDTO
       {
           Id = sp.Size.Id,
           Size = sp.Size.Content,
           StockCount = sp.StockQuantity
       }).ToList(),
       RelatedProducts = _context.Products.Where(p => p.CategoryId == x.CategoryId && p.Id != x.Id)
           .Select(p => new GetProductDTO
           {
               Id = p.Id,
               ProductCode = p.ProductCode,
               Price = p.Price,
               Discount = p.DisCount,
               Title = p.ProductLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Title,
               CategoryName = p.Category.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Name,
               ImageUrl = p.Images[0].Path
           }).ToList()
   })

      .Where(x => x.Id == id)
   .FirstOrDefaultAsync();


                return productQuery is null ?
                  new ErrorDataResult<GetProductDetailDTO>(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound) :
                    new SuccessDataResult<GetProductDetailDTO>(data: productQuery,LangCode, HttpStatusCode.OK);


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorDataResult<GetProductDetailDTO>(LangCode,message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
          
        }
        public async Task<IResult> UpdateProductAsync(UpdateProductDTO updateProductDTO, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.BadRequest);

            var validationRules = new UpdateProductDTOValidation(LangCode, SupportedLaunguages);
            var validationResult = validationRules.Validate(updateProductDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(LangCode,messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);

            // Find product to update
            var product = await _context.Products
                .Include(p => p.ProductLanguages)
                .Include(p => p.SizeProducts)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == updateProductDTO.Id);

            if (product == null)
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            Category? category = _context.Categories.AsNoTracking().FirstOrDefault(x => x.Id == updateProductDTO.CategoryId);
            if (category is null)
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            // Update basic fields
            product.ProductCode = updateProductDTO.ProductCode;
            product.Price = updateProductDTO.Price;
            product.DisCount = updateProductDTO.Discount;
            product.IsFeatured = updateProductDTO.Isfeature;
            product.CategoryId = category.Id;

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
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], statusCode: HttpStatusCode.NotFound);

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

            if (updateProductDTO.DeletedImageUrls != null && updateProductDTO.DeletedImageUrls.Any())
            {
                foreach (var urlToDelete in updateProductDTO.DeletedImageUrls)
                {
            Image? deletedImage=product.Images.FirstOrDefault(img => img.Path == urlToDelete);
                    if (deletedImage is not  null)
                    {
                     
                        var fileRemovalResult = _fileService.RemoveFile(LangCode,deletedImage.Path);
                       
                        if (!fileRemovalResult.IsSuccess)
                            return new ErrorResult(LangCode,message: fileRemovalResult.Message, fileRemovalResult.StatusCode);
                        _context.Images.Remove(deletedImage);
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
                        var imageUrlResult = await _fileService.SaveImageAsync(LangCode,newImage, true);
                        if (!imageUrlResult.IsSuccess)
                            return new ErrorResult(LangCode,message: HttpStatusErrorMessages.FileUploadFailed[LangCode], HttpStatusCode.InternalServerError);

                        Image newPicture = new Image
                        {
                            Path = imageUrlResult.Data,
                            ProductId = product.Id
                        };

                      _context.Images.Add(newPicture);

                    }
                }
            }

            _context.Products.Update(product);

            try
            {
                await _context.SaveChangesAsync();
                return new SuccessResult(LangCode,message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
        }

        public async Task<IDataResult<GetProductForUpdateDTO>> GetProductByIdForUpdateAsync(Guid id, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<GetProductForUpdateDTO>(LangCode,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.BadRequest);
            if (id == Guid.Empty)
                return new ErrorDataResult<GetProductForUpdateDTO>(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            var product = await _context.Products.AsNoTracking().Where(x => x.Id == id)
                .Select(x => new GetProductForUpdateDTO
                {
                    Id = x.Id,
                    ProductCode = x.ProductCode,
                    Price = x.Price,
                    IsFeature = x.IsFeatured,
                    CategoryId = x.CategoryId,
                    Discount = x.DisCount,
                    ImageUrls = x.Images.Select(i => i.Path).ToList(),
                    Title = x.ProductLanguages.Select(x => new KeyValuePair<string, string>(x.LanguageCode, x.Title)).ToDictionary(),
                    Description = x.ProductLanguages.Select(x => new KeyValuePair<string, string>(x.LanguageCode, x.Description)).ToDictionary(),
                    Sizes=x.SizeProducts.Select(sp => new GetSizeForProductDetailDTO
                    {
                        Id = sp.Size.Id,
                        Size = sp.Size.Content,
                        StockCount = sp.StockQuantity
                    }).ToList()

                }).FirstOrDefaultAsync();
            if (product is null)           
                return new ErrorDataResult<GetProductForUpdateDTO>(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            return new SuccessDataResult<GetProductForUpdateDTO>(data: product, LangCode,message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);

        }
    }
}
