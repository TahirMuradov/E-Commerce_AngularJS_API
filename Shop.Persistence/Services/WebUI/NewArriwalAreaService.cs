using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services.WebUI;
using Shop.Application.DTOs.WebUI.NewArriwalDTOs;
using Shop.Application.ResultTypes.Abstract;
using Shop.Persistence.Context;

namespace Shop.Persistence.Services.WebUI
{
    public class NewArriwalAreaService : INewArriwalAreaService
    {
        private readonly ILogger<NewArriwalAreaService> _logger;
        private readonly AppDBContext _context;
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
            throw new NotImplementedException();
        }

        public IDataResult<IQueryable<GetNewArriwalProductDTO>> GetNewArriwalProducts(string LangCode)
        {
            throw new NotImplementedException();
        }
    }
}
