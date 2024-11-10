using eShop.Basket.Contracts.Grpc;
using static eShop.Basket.Contracts.Grpc.Basket;

namespace eShop.ServiceInvocation.BasketApiClient.Refit;

public class BasketApiClient(BasketClient basketClient) : IBasketApiClient
{
    public async Task DeleteBasketAsync()
    {
        await basketClient.DeleteBasketAsync(new DeleteBasketRequest());
    }

    public async Task<CustomerBasketResponse> GetBasketAsync()
    {
        return await basketClient.GetBasketAsync(new());
    }

    public async Task UpdateBasketAsync(UpdateBasketRequest request)
    {
        await basketClient.UpdateBasketAsync(request);
    }
}
