using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Abstraction.Services;
using Shop.Infrastructure.Services;


namespace Shop.Infrastructure
{
   public static class ServiceRegistration
    {
      

      
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {

            // serviceCollection.Scan(scan => scan
            //.FromAssemblyOf<ITokenHandler>()
            //.AddClasses(classes => classes.InNamespaces("Shop.Persistence.Services"))
            //.AsImplementedInterfaces()
            //.WithScopedLifetime());

            //serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
            //serviceCollection .AddScoped<IFileService,FileService>();
            //serviceCollection.AddScoped<IMailService, MailServce>();

            serviceCollection.AddSingleton<IGetRequestLangService, GetRequestLangService>();




        }

    }
}
