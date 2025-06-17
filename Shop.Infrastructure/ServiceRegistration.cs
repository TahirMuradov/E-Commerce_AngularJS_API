using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Abstraction.Services;
using Shop.Application.Abstraction.Token;
using Shop.Infrastructure.Services;
using Shop.Infrastructure.Services.Token;

namespace Shop.Infrastructure
{
   public static class ServiceRegistration
    {
      

      
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {



            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
            serviceCollection .AddScoped<IFileService,FileService>();
            serviceCollection.AddScoped<IMailService, MailServce>();
            serviceCollection.AddSingleton<IGetRequestLangService, GetRequestLangService>();



        }

    }
}
