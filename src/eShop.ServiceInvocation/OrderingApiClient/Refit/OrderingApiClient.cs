using eShop.Ordering.Contracts;
using eShop.Ordering.Contracts.GetCardTypes;

namespace eShop.ServiceInvocation.OrderingApiClient.Refit;

public class OrderingApiClient(IOrderingApi orderingApi) : IOrderingApiClient
{
    public async Task Cancel(Guid objectId)
    {
        await orderingApi.Cancel(objectId);
    }

    public async Task ConfirmGracePeriod(Guid objectId)
    {
        await orderingApi.ConfirmGracePeriod(objectId);
    }

    public async Task ConfirmStock(Guid objectId)
    {
        await orderingApi.ConfirmStock(objectId);
    }

    public async Task<Guid> CreateOrder(Guid requestId, Ordering.Contracts.CreateOrder.OrderDto dto)
    {
        return await orderingApi.CreateOrder(requestId, dto);
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

    public async Task Paid(Guid objectId)
    {
        await orderingApi.Paid(objectId);
    }

    public async Task RejectStock(Guid objectId, Guid[] orderStockItems)
    {
        await orderingApi.RejectStock(objectId, orderStockItems);
    }

    public async Task UpdateOrder(Guid objectId, Ordering.Contracts.UpdateOrder.OrderDto dto)
    {
        await orderingApi.UpdateOrder(objectId, dto);
    }
}
