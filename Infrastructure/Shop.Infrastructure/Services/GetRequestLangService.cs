using Microsoft.AspNetCore.Http;
using Shop.Application.Abstraction.Services;
using Shop.Infrastructure.Utilities;


namespace Shop.Infrastructure.Services
{
   public class GetRequestLangService: IGetRequestLangService
    {

    
        public string GetRequestLanguage()
        {


            return Thread.CurrentThread.CurrentCulture.Name;
        }
    }
}
