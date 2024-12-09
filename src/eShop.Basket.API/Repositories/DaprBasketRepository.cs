using Ardalis.Result;
using Dapr.Client;
using eShop.Basket.API.Model;
using eShop.Shared.Features;

namespace eShop.Basket.API.Repositories;

public class DaprBasketRepository(DaprClient daprClient, IConfiguration configuration,
    ILogger<DaprBasketRepository> logger) : IBasketRepository
{
    private readonly string storeName = "statestore";    
    private FeaturesConfiguration? features => configuration.GetSection("Features").Get<FeaturesConfiguration>();

    public async Task<Result> DeleteBasketAsync(string id)
    {
        try
        {
            await daprClient.DeleteStateAsync(this.storeName, id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to delete basket";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result<CustomerBasket>> GetBasketAsync(string customerId)
    {
        try
        {
            CustomerBasket basket = await daprClient.GetStateAsync<CustomerBasket>(this.storeName, customerId);
            return basket;
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to get basket";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result<CustomerBasket>> UpdateBasketAsync(CustomerBasket basket)
    {
        try
        {
            await daprClient.SaveStateAsync(this.storeName, basket.BuyerId, basket);
            return basket;
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to update basket";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
