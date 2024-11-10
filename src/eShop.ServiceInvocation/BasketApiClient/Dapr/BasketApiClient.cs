using eShop.Basket.Contracts.Grpc;
using Grpc.Core;
using static eShop.Basket.Contracts.Grpc.Basket;

namespace eShop.ServiceInvocation.BasketApiClient.Dapr;

public class BasketApiClient(BasketClient basketClient) : IBasketApiClient
{
    private readonly Metadata headers = new() { { "dapr-app-id", "basket-api" } };

    public async Task DeleteBasketAsync()
    {
        await basketClient.DeleteBasketAsync(new(), this.headers);
    }

    public async Task<CustomerBasketResponse> GetBasketAsync()
    {
        return await basketClient.GetBasketAsync(new(), this.headers);
    }

    public async Task UpdateBasketAsync(UpdateBasketRequest request)
    {
        await basketClient.UpdateBasketAsync(request, this.headers);
    }
}
