using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services.WebUI;
using Shop.Application.DTOs.WebUI;
using Shop.Application.DTOs.WebUI.DisCountAreaDTOs;
using Shop.Application.DTOs.WebUI.HomeSliderItemDTOs;
using Shop.Application.DTOs.WebUI.NewArriwalDTOs;
using Shop.Application.DTOs.WebUI.TopCategoryAreaDTOs;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Domain.Exceptions;
using Shop.Persistence.Context;
using System.Net;

namespace Shop.Persistence.Services.WebUI
{
    public class HomeService : IHomeService
    {
        private readonly ILogger<HomeService> _logger;
        private readonly AppDBContext _context;

        public HomeService(ILogger<HomeService> logger, AppDBContext context)
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
        public IDataResult<GetHomeAllDataDTO> GetHomeAllData(string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<GetHomeAllDataDTO>(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);

            IQueryable<GetDisCountAreaForUiDTO> DisCountAreas = _context.DisCountAreas.AsNoTracking().AsSplitQuery().Select(x => new GetDisCountAreaForUiDTO
            {

                Title = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Title).FirstOrDefault(),
                Description = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Description).FirstOrDefault(),

            });
            IQueryable<GetHomeSliderItemForUIDTO> HomeSliderItems = _context.HomeSliderItems.AsNoTracking().AsSplitQuery().Select(x => new GetHomeSliderItemForUIDTO
            {
                Title = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Title).FirstOrDefault(),
                Description = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Description).FirstOrDefault(),
                ImageUrl = x.Image.Path
            });

            IQueryable<GetTopCategoryAreaForUIDTO> TopCategoryAreas = _context.TopCategoryAreas.AsNoTracking().AsSplitQuery().Select(x => new GetTopCategoryAreaForUIDTO
            {
                Title = x.TopCategoryAreaLanguages.FirstOrDefault(y => y.LangCode == LangCode).Title,
                Description = x.TopCategoryAreaLanguages.FirstOrDefault(y => y.LangCode == LangCode).Description,
                PictureUrl = x.Image.Path,
                CategoryId = x.CategoryId.ToString(),
            });
            IQueryable<GetNewArriwalProductDTO> NewArriwalProducts = _context.Products.AsNoTracking().AsSplitQuery().Select(x => new GetNewArriwalProductDTO
            {
                Id = x.Id,
                Title = x.ProductLanguages.Where(y => y.LanguageCode == LangCode).Select(s => s.Title).FirstOrDefault(),
                Price = x.Price,
                DisCount = x.DisCount,
                ImgUrl = x.Images.Select(sl => sl.Path).FirstOrDefault(),
                Category = new GetIsFeaturedCategoryDTO
                {
                    Id = x.CategoryId,
                    CategoryName = x.Category.CategoryLanguages.Where(y => y.LanguageCode == LangCode).Select(s => s.Name).FirstOrDefault()
                }



            });
            IQueryable<GetIsFeaturedCategoryDTO> NewArriwalCategory = _context.Categories.AsNoTracking().AsSplitQuery().Where(x => x.IsFeatured).Select(x => new GetIsFeaturedCategoryDTO
            {
                Id = x.Id,
                CategoryName = x.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == LangCode).Name
            });
            return new SuccessDataResult<GetHomeAllDataDTO>(data: new GetHomeAllDataDTO
            {
                DisCountAreas = DisCountAreas,
                HomeSliderItems = HomeSliderItems,
                TopCategoryAreas = TopCategoryAreas,
                NewArriwalProducts = NewArriwalProducts,
                IsFeaturedCategorys = NewArriwalCategory
            }, message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);

        }
    }
}
