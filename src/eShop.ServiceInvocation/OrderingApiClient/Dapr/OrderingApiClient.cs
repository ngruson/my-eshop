using Dapr.Client;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.ServiceInvocation.Auth;

namespace eShop.ServiceInvocation.OrderingApiClient.Dapr;

public class OrderingApiClient(DaprClient daprClient, AccessTokenAccessorFactory accessTokenAccessorFactory)
    : BaseDaprApiClient(daprClient, accessTokenAccessorFactory), IOrderingApiClient
{
    private readonly string basePath = "/api/orders";
    protected override string AppId => "ordering-api";

    public async Task Cancel(Guid objectId)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            $"{this.basePath}/cancel/{objectId}");

        await this.DaprClient.InvokeMethodAsync(request);
    }

    public async Task ConfirmGracePeriod(Guid objectId)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            $"{this.basePath}/confirmGracePeriod/{objectId}");

        await this.DaprClient.InvokeMethodAsync(request);
    }

    public async Task ConfirmStock(Guid objectId)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            $"{this.basePath}/confirmStock/{objectId}");

        await this.DaprClient.InvokeMethodAsync(request);
    }

    public async Task<Guid> CreateOrder(Guid requestId, Ordering.Contracts.CreateOrder.OrderDto dto)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            this.basePath,
            null,
            dto);

        request.Headers.Add("x-requestid", requestId.ToString());

        return await this.DaprClient.InvokeMethodAsync<Guid>(request);
    }

    public async Task DeleteOrder(Guid objectId)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Delete,
            $"{this.basePath}/{objectId}");

        await this.DaprClient.InvokeMethodAsync(request);
    }

    public async Task<Ordering.Contracts.GetOrders.OrderDto[]> GetOrders()
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            $"{this.basePath}/all");

        return await this.DaprClient.InvokeMethodAsync<Ordering.Contracts.GetOrders.OrderDto[]>(request);
    }

    public async Task<Ordering.Contracts.GetOrder.OrderDto> GetOrder(Guid objectId)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            $"{this.basePath}/{objectId}");

        return await this.DaprClient.InvokeMethodAsync<Ordering.Contracts.GetOrder.OrderDto>(request);
    }

    public async Task<CardTypeDto[]> GetCardTypes()
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            $"{this.basePath}/cardTypes");

        return await this.DaprClient.InvokeMethodAsync<CardTypeDto[]>(request);
    }

    public async Task Paid(Guid objectId)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            $"{this.basePath}/paid/{objectId}");

        await this.DaprClient.InvokeMethodAsync(request);
    }

    public async Task RejectStock(Guid objectId, Guid[] orderStockItems)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            $"{this.basePath}/rejectStock/{objectId}",
            null,
            orderStockItems);

        await this.DaprClient.InvokeMethodAsync(request);
    }

    public async Task UpdateOrder(Guid objectId, Ordering.Contracts.UpdateOrder.OrderDto dto)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Put,
            $"{this.basePath}/{objectId}",
            null,
            dto);

        await this.DaprClient.InvokeMethodAsync(request);
    }
}
