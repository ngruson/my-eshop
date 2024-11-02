using eShop.Basket.API.Grpc;
using GrpcBasketItem = eShop.Basket.API.Grpc.BasketItem;
using GrpcBasketClient = eShop.Basket.API.Grpc.Basket.BasketClient;

namespace eShop.WebApp.Services;

public class BasketService(GrpcBasketClient basketClient)
{
    public async Task<IReadOnlyCollection<BasketQuantity>> GetBasketAsync()
    {
        CustomerBasketResponse result = await basketClient.GetBasketAsync(new());
        return MapToBasket(result);
    }

    public async Task DeleteBasketAsync()
    {
        await basketClient.DeleteBasketAsync(new DeleteBasketRequest());
    }

    public async Task UpdateBasketAsync(IReadOnlyCollection<BasketQuantity> basket)
    {
        UpdateBasketRequest updatePayload = new();

        foreach (BasketQuantity item in basket)
        {
            GrpcBasketItem updateItem = new GrpcBasketItem
            {
                ProductId = item.ProductId.ToString(),
                Quantity = item.Quantity,
            };
            updatePayload.Items.Add(updateItem);
        }

        await basketClient.UpdateBasketAsync(updatePayload);
    }

    private static List<BasketQuantity> MapToBasket(CustomerBasketResponse response)
    {
        List<BasketQuantity> result = [];
        foreach (GrpcBasketItem? item in response.Items)
        {
            result.Add(new BasketQuantity(Guid.Parse(item.ProductId), item.Quantity));
        }

        return result;
    }
}

public record BasketQuantity(Guid ProductId, int Quantity);
