using Microsoft.AspNetCore.Http;
using Shop.Application.Abstraction.Services;
using Shop.Infrastructure.Utilities;


namespace Shop.Infrastructure.Services
{
   public class GetRequestLangService: IGetRequestLangService
    {

        private readonly IHttpContextAccessor _contextAccessor;

        public GetRequestLangService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
          
        }
        public string GetRequestLanguage()
        {
            var supportedLanguages = ConfigurationInfrastructure.SupportedLaunguageKeys;
            var defaultLanguage = ConfigurationInfrastructure.DefaultLanguageKey??"az";

            var headerLang = _contextAccessor.HttpContext?.Request?.Headers["Accept-Language"].ToString();
             //en-US,en; q=0.9,ru; q=0.8,az; q=0.7
            if (string.IsNullOrWhiteSpace(headerLang)||headerLang.Contains("q="))
                return defaultLanguage;
            var matchedLang = supportedLanguages.FirstOrDefault(ln=>ln==headerLang);

            return matchedLang ?? defaultLanguage;
        }
    }
}
