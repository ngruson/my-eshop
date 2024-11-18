using eShop.Basket.Contracts.Grpc;

namespace eShop.ServiceInvocation.BasketApiClient;

public interface IBasketApiClient
{
    Task DeleteBasketAsync();
    Task<CustomerBasketResponse> GetBasketAsync();
    Task UpdateBasketAsync(UpdateBasketRequest request);
}
