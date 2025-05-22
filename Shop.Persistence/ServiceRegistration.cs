
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Abstraction.Services;
using Shop.Application.Exceptions;
using Shop.Domain.Entities;
using Shop.Infrastructure;
using Shop.Persistence.Context;

namespace Shop.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
   .AddEntityFrameworkStores<AppDBContext>()
   .AddDefaultTokenProviders()
      .AddErrorDescriber<MultilanguageIdentityErrorDescriber>();
            services.AddScoped<IAuthServices, AuthService>();






        }
    }
}