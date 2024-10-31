using eShop.Ordering.Contracts;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Ordering.Contracts.GetOrders;

namespace eShop.ServiceInvocation.OrderingApiClient.Refit;

public class OrderingApiClient(IOrderingApi orderingApi) : IOrderingApiClient
{
    public Task<OrderDto[]> GetOrders()
    {
        return orderingApi.GetOrders();
    }

    public async Task CreateOrder(Guid requestId, CreateOrderDto dto)
    {
        await orderingApi.CreateOrder(requestId, dto);
    }

    public async Task<CardTypeDto[]> GetCardTypes()
    {
        return await orderingApi.GetCardTypes();
    }
}
