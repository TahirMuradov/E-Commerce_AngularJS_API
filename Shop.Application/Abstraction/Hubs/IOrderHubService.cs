namespace Shop.Application.Abstraction.Hubs
{
   public interface IOrderHubService
    {
      public  Task OrderAddedMessageAsync(string message);
    }
}
