using Dapr.Client;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Shared.Auth;

namespace eShop.ServiceInvocation.OrderingApiClient.Dapr;

public class OrderingApiClient(DaprClient daprClient, AccessTokenAccessor accessTokenAccessor)
    : BaseDaprApiClient(daprClient, accessTokenAccessor), IOrderingApiClient
{
    private readonly string basePath = "/api/orders";
    protected override string AppId => "ordering-api";

    public async Task CreateOrder(Guid requestId, Ordering.Contracts.CreateOrder.OrderDto dto)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            this.basePath,
            null,
            dto);

        request.Headers.Add("x-requestid", requestId.ToString());

        await this.DaprClient.InvokeMethodAsync(request);
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
