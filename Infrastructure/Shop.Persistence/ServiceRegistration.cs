﻿
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shop.Domain.Entities;
using Shop.Domain.Exceptions;
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

            services.Configure<IdentityOptions>(options =>
            {
                // Default User settings.
                options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(15);
            });




            //services.Scan(scan => scan
            //    .FromAssemblyOf<ICategoryService>()
            //    .AddClasses(classes => classes.InNamespaces("Shop.Persistence.Services"))
            //    .AsImplementedInterfaces()
            //    .WithScopedLifetime());



            //services.AddScoped<ICategoryService, CategoryService>();
            //services.AddScoped<ISizeService, SizeService>();
            //services.AddScoped<IShippingMethodService, ShippingMethodService>();
            //services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            //services.AddScoped<IProductService, ProductService>();
            //services.AddScoped<IOrderService, OrderService>();


            //WebUIService
            //services.AddScoped<IDiscountAreaService, DisCountAreaService>();
            //services.AddScoped<IHomeSliderService, HomeSliderService>();
            //services.AddScoped<IHomeService, HomeService>();
            //services.AddScoped<ITopCategoryAreaService, TopCategoryAreaService>();
            //services.AddScoped<INewArriwalAreaService, NewArriwalAreaService>();






        }
    }
}