using Dapr.Client;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Ordering.Contracts.GetOrders;
using eShop.Shared.Auth;

namespace eShop.ServiceInvocation.OrderingApiClient.Dapr;

public class OrderingApiClient(DaprClient daprClient, AccessTokenAccessor accessTokenAccessor)
    : BaseDaprApiClient(daprClient, accessTokenAccessor), IOrderingApiClient
{
    private readonly string basePath = "/api/orders";
    protected override string AppId => "ordering-api";

    public async Task CreateOrder(Guid requestId, CreateOrderDto dto)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            this.basePath,
            null,
            dto);

        request.Headers.Add("x-requestid", requestId.ToString());

        await this.DaprClient.InvokeMethodAsync(request);
    }

    public async Task<CardTypeDto[]> GetCardTypes()
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            $"{this.basePath}/cardTypes");

        return await this.DaprClient.InvokeMethodAsync<CardTypeDto[]>(request);
    }

    public async Task<OrderDto[]> GetOrders()
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            $"{this.basePath}/all");

        return await this.DaprClient.InvokeMethodAsync<OrderDto[]>(request);
    }
}
