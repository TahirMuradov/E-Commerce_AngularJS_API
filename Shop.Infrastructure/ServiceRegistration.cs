using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Abstraction.Token;
using Shop.Infrastructure.Services.Token;
using Shop.Infrastructure.Services;

namespace Shop.Infrastructure
{
   public static class ServiceRegistration
    {

        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
           
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
            //serviceCollection.AddScoped<IMailService, MailService>();
      
        }

    }
}
