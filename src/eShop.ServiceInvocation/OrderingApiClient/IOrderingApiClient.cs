namespace eShop.ServiceInvocation.OrderingApiClient;

public interface IOrderingApiClient
{
    Task<Ordering.Contracts.GetOrders.OrderDto[]> GetOrders();
    Task<Ordering.Contracts.GetOrder.OrderDto> GetOrder(Guid objectId);
    Task CreateOrder(Guid requestId, Ordering.Contracts.CreateOrder.OrderDto dto);
    Task DeleteOrder(Guid objectId);
    Task<Ordering.Contracts.GetCardTypes.CardTypeDto[]> GetCardTypes();
    Task UpdateOrder(Guid objectId, Ordering.Contracts.UpdateOrder.OrderDto dto);
}
