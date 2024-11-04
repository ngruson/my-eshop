using eShop.Ordering.Contracts;
using eShop.Ordering.Contracts.GetCardTypes;

namespace eShop.ServiceInvocation.OrderingApiClient.Refit;

public class OrderingApiClient(IOrderingApi orderingApi) : IOrderingApiClient
{
    public async Task CreateOrder(Guid requestId, Ordering.Contracts.CreateOrder.OrderDto dto)
    {
        await orderingApi.CreateOrder(requestId, dto);
    }

    public async Task DeleteOrder(Guid objectId)
    {
        await orderingApi.DeleteOrder(objectId);
    }

    public Task<Ordering.Contracts.GetOrders.OrderDto[]> GetOrders()
    {
        return orderingApi.GetOrders();
    }

    public async Task<Ordering.Contracts.GetOrder.OrderDto> GetOrder(Guid objectId)
    {
        return await orderingApi.GetOrder(objectId);
    }

    public async Task<CardTypeDto[]> GetCardTypes()
    {
        return await orderingApi.GetCardTypes();
    }

    public async Task UpdateOrder(Guid objectId, Ordering.Contracts.UpdateOrder.OrderDto dto)
    {
        await orderingApi.UpdateOrder(objectId, dto);
    }
}
