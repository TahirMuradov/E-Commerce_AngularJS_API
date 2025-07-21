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
        public IDataResult<IQueryable<GetIsFeaturedCategoryDTO>> GetNewArriwalCategories(string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<IQueryable<GetIsFeaturedCategoryDTO>>(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.BadRequest);
            IQueryable<GetIsFeaturedCategoryDTO> categories =  _context.Categories.AsNoTracking().AsSplitQuery().Where(x => x.IsFeatured).Select(x => new GetIsFeaturedCategoryDTO
            {
                Id = x.Id,
                CategoryName = x.CategoryLanguages.Where(y => y.LanguageCode == LangCode).Select(s => s.Name).FirstOrDefault(),
                
            }) ;
            return new SuccessDataResult<IQueryable<GetIsFeaturedCategoryDTO>>(categories, LangCode,message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);

        }

        public IDataResult<IQueryable<GetNewArriwalProductDTO>> GetNewArriwalProducts(string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<IQueryable<GetNewArriwalProductDTO>>(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.BadRequest);
            IQueryable<GetNewArriwalProductDTO> categories = _context.Products.AsNoTracking().AsSplitQuery().Where(x => x.IsFeatured).Select(x => new GetNewArriwalProductDTO
            {
                Id = x.Id,
                Title = x.ProductLanguages.Where(y => y.LanguageCode == LangCode).Select(s => s.Title).FirstOrDefault(),
                Price = x.Price,
                DisCount = x.DisCount,
                ImgUrl = x.Images.Select(sl=>sl.Path).FirstOrDefault(),
                Category = new GetIsFeaturedCategoryDTO
                {
                    Id = x.CategoryId,
                    CategoryName = x.Category.CategoryLanguages.Where(y => y.LanguageCode == LangCode).Select(s => s.Name).FirstOrDefault()
                },
                

            });
            return new SuccessDataResult<IQueryable<GetNewArriwalProductDTO>>(categories, LangCode,message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
        }
    }
}
