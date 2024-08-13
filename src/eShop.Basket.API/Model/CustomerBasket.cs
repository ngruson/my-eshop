using eShop.Shared.Data;

namespace eShop.Basket.API.Model;

public class CustomerBasket : IAggregateRoot
{
    public required string BuyerId { get; set; }

    public List<BasketItem> Items { get; set; } = [];

    public CustomerBasket() { }
}
