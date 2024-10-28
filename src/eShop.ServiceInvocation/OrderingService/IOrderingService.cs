using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Ordering.Contracts.GetOrders;

namespace eShop.ServiceInvocation.OrderingService;

public interface IOrderingService
{
    Task<OrderDto[]> GetOrders();
    Task CreateOrder(Guid requestId, CreateOrderDto request);
    Task<IEnumerable<CardTypeDto>> GetCardTypes();
}
