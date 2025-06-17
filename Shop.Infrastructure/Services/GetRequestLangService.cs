using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shop.Application.Abstraction.Services;

namespace Shop.Infrastructure.Services
{
   public class GetRequestLangService: IGetRequestLangService
    {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        public GetRequestLangService(IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }
        public string GetRequestLanguage()
        {
            var supportedLanguages = _configuration.GetSection("SupportedLanguage:Launguages").Get<List<string>>();
            var defaultLanguage = _configuration.GetValue<string>("SupportedLanguage:Default") ?? "az";

            var headerLang = _contextAccessor.HttpContext?.Request?.Headers["Accept-Language"].ToString();

            if (string.IsNullOrWhiteSpace(headerLang))
                return defaultLanguage;

            var acceptedLanguages = headerLang
                .Split(',')
                .Select(lang => lang.Split(';')[0])        
                .Select(lang => lang.Split('-')[0].Trim().ToLower())  
                .ToList();

            var matchedLang = acceptedLanguages.FirstOrDefault(lang => supportedLanguages.Contains(lang));

            return matchedLang ?? defaultLanguage;
        }
    }
}
