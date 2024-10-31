using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Ordering.Contracts.GetOrders;

namespace eShop.ServiceInvocation.OrderingApiClient;

public interface IOrderingApiClient
{
    Task<OrderDto[]> GetOrders();
    Task CreateOrder(Guid requestId, CreateOrderDto dto);
    Task<CardTypeDto[]> GetCardTypes();
}
