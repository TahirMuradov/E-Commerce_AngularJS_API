
using System.Globalization;

namespace ShopAPI.Middlewares
{
    public class LocalizationMiddleware : IMiddleware
    {
       
        private readonly string[] _supportedCultures;
        private readonly string _defaultCulture ;
        public LocalizationMiddleware(IConfiguration configuration)
        {
           
       _supportedCultures = configuration.GetSection("SupportedLanguage:Launguages").Get<string[]>();
            _defaultCulture = configuration.GetSection("SupportedLanguage:Default").Get<string>();
        }
       
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var lang = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            if (_supportedCultures.Contains(lang))
            {
                
            var culture = new CultureInfo(lang);


                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                return next(context);
            }
            else
            {
                var defaultCulture = new CultureInfo(_defaultCulture);
                Thread.CurrentThread.CurrentCulture = defaultCulture;
                Thread.CurrentThread.CurrentUICulture = defaultCulture;
                return next(context);
            }

            
            
        }
    }
}
