using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services.WebUI;
using Shop.Application.DTOs.WebUI.NewArriwalDTOs;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Domain.Exceptions;
using Shop.Persistence.Context;
using System.Net;
using System.Threading.Tasks;

namespace Shop.Persistence.Services.WebUI
{
    public class NewArriwalAreaService : INewArriwalAreaService
    {
        private readonly ILogger<NewArriwalAreaService> _logger;
        private readonly AppDBContext _context;

        public NewArriwalAreaService(ILogger<NewArriwalAreaService> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

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
        public IDataResult<IQueryable<GetIsFeaturedCategoryDTO>> GetNewArriwalCategories(string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<IQueryable<GetIsFeaturedCategoryDTO>>(message: HttpStatusErrorMessages.UnsupportedLanguage[LangCode], HttpStatusCode.BadRequest);
            IQueryable<GetIsFeaturedCategoryDTO> categories =  _context.Categories.AsNoTracking().AsSplitQuery().Where(x => x.IsFeatured).Select(x => new GetIsFeaturedCategoryDTO
            {
                CategoryId = x.Id,
                CategoryName = x.CategoryLanguages.Where(y => y.LanguageCode == LangCode).Select(s => s.Name).FirstOrDefault(),
                
            }) ;
            return new SuccessDataResult<IQueryable<GetIsFeaturedCategoryDTO>>(categories, message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);

        }

        public IDataResult<IQueryable<GetNewArriwalProductDTO>> GetNewArriwalProducts(string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<IQueryable<GetNewArriwalProductDTO>>(message: HttpStatusErrorMessages.UnsupportedLanguage[LangCode], HttpStatusCode.BadRequest);
            IQueryable<GetNewArriwalProductDTO> categories = _context.Products.AsNoTracking().AsSplitQuery().Where(x => x.IsFeatured).Select(x => new GetNewArriwalProductDTO
            {
                Id = x.Id,
                Title = x.ProductLanguages.Where(y => y.LanguageCode == LangCode).Select(s => s.Title).FirstOrDefault(),
                Price = x.Price,
                DisCount = x.DisCount,
                ImgUrls = x.ImageUrls,
                Category = new GetIsFeaturedCategoryDTO
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.CategoryLanguages.Where(y => y.LanguageCode == LangCode).Select(s => s.Name).FirstOrDefault()
                },
                

            });
            return new SuccessDataResult<IQueryable<GetNewArriwalProductDTO>>(categories, message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
        }
    }
}
