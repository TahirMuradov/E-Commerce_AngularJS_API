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

            // Accept-Language genelde şöyle gelir: "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7"
            // O yüzden ilk parçaları virgülle ayır, sonra dil kodlarını normalize et (küçük harfe çevir ve - sonrası varsa almayı bırak)
            var acceptedLanguages = headerLang
                .Split(',')
                .Select(lang => lang.Split(';')[0])         // "ru-RU;q=0.9" -> "ru-RU"
                .Select(lang => lang.Split('-')[0].Trim().ToLower())  // "ru-RU" -> "ru"
                .ToList();

            // Desteklenen ilk dili bul
            var matchedLang = acceptedLanguages.FirstOrDefault(lang => supportedLanguages.Contains(lang));

            return matchedLang ?? defaultLanguage;
        }
    }
}
