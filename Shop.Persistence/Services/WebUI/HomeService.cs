using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services.WebUI;
using Shop.Application.DTOs.WebUI;
using Shop.Application.ResultTypes.Abstract;
using Shop.Persistence.Context;

namespace Shop.Persistence.Services.WebUI
{
    public class HomeService : IHomeService
    {
        private readonly ILogger<HomeService> _logger;
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
        public IDataResult<GetHomeAllDataDTO> GetHomeAllData(string LangCode)
        {
            throw new NotImplementedException();
        }
    }
}
