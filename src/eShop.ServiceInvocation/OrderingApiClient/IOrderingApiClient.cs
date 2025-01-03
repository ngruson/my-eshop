namespace eShop.ServiceInvocation.OrderingApiClient;

public interface IOrderingApiClient
{
    Task Cancel(Guid objectId);
    Task ConfirmGracePeriod(Guid objectId);
    Task ConfirmStock(Guid objectId);    
    Task<Guid> CreateOrder(Guid requestId, Ordering.Contracts.CreateOrder.OrderDto dto);
    Task DeleteOrder(Guid objectId);
    Task<Ordering.Contracts.GetCardTypes.CardTypeDto[]> GetCardTypes();
    Task<Ordering.Contracts.GetOrders.OrderDto[]> GetOrders();
    Task<Ordering.Contracts.GetOrder.OrderDto> GetOrder(Guid objectId);
    Task Paid(Guid objectId);
    Task RejectStock(Guid objectId, Guid[] orderStockItems);
    Task UpdateOrder(Guid objectId, Ordering.Contracts.UpdateOrder.OrderDto dto);
}
