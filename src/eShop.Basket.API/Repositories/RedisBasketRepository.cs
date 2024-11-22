using System.Text.Json.Serialization;
using Ardalis.Result;
using eShop.Basket.API.Model;

namespace eShop.Basket.API.Repositories;

public class RedisBasketRepository(ILogger<RedisBasketRepository> logger, IConnectionMultiplexer redis) : IBasketRepository
{
    private readonly IDatabase _database = redis.GetDatabase();

    // implementation:

    // - /basket/{id} "string" per unique basket
    private static readonly RedisKey BasketKeyPrefix = "/basket/"u8.ToArray();
    // note on UTF8 here: library limitation (to be fixed) - prefixes are more efficient as blobs

    private static RedisKey GetBasketKey(string userId) => BasketKeyPrefix.Append(userId);

    public async Task<Result> DeleteBasketAsync(string id)
    {
        try
        {
            await this._database.KeyDeleteAsync(GetBasketKey(id));
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
            RedisValue data = await this._database.StringGetAsync(GetBasketKey(customerId));

            if (data == RedisValue.Null)
            {
                return Result.NotFound();
            }
            return JsonSerializer.Deserialize(data!, BasketSerializationContext.Default.CustomerBasket)!;
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to delete basket";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<Result<CustomerBasket>> UpdateBasketAsync(CustomerBasket basket)
    {
        try
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(basket, BasketSerializationContext.Default.CustomerBasket);
            bool created = await this._database.StringSetAsync(GetBasketKey(basket.BuyerId), json);

            if (!created)
            {
                logger.LogInformation("Problem occurred persisting the item.");
                return Result.Error();
            }

            logger.LogInformation("Basket item persisted successfully.");
            return await this.GetBasketAsync(basket.BuyerId);
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to delete basket";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}

[JsonSerializable(typeof(CustomerBasket))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
public partial class BasketSerializationContext : JsonSerializerContext
{

}
