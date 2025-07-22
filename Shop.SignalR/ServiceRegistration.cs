using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Abstraction.Hubs;
using Shop.SignalR.HubServices;

namespace Shop.SignalR
{
   public static class ServiceRegistration
    {
        public static void AddSignalRServices(this IServiceCollection collection)
        {
            collection.AddTransient<IProductHubService, ProductHubService>();
            collection.AddTransient<IOrderHubService, OrderHubService>();
            collection.AddSignalR();
        }
    }
}
