using Microsoft.AspNetCore.SignalR;
using Shop.Application.Abstraction.Hubs;
using Shop.Persistence.Context;
using Shop.SignalR.Hubs;

namespace Shop.SignalR.HubServices
{
    public class OrderHubService : IOrderHubService
    {
        readonly IHubContext<OrderHub> _hubContext;
        readonly AppDBContext _appDBContext;

        public OrderHubService(IHubContext<OrderHub> hubContext, AppDBContext appDBContext)
        {
            _hubContext = hubContext;
            _appDBContext = appDBContext;
        }

        public async Task OrderAddedMessageAsync(string message)
            => await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.OrderAddedMessage, message);
    }
}
