using eShop.Ordering.Contracts;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Ordering.Contracts.GetOrders;

namespace eShop.ServiceInvocation.OrderingService.Refit;

public class OrderingService(IOrderingApi orderingApi) : IOrderingService
{
    public Task<OrderDto[]> GetOrders()
    {
        return orderingApi.GetOrders();
    }

    public async Task CreateOrder(Guid requestId, CreateOrderDto request)
    {
        await orderingApi.CreateOrder(requestId, request);
    }

    public async Task<IEnumerable<CardTypeDto>> GetCardTypes()
    {
        return await orderingApi.GetCardTypes();
    }
}
