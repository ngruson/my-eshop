using eShop.WebAppComponents.Services.ViewModels;

namespace eShop.WebApp.Services;

public interface IBasketState
{
    public Task<IReadOnlyCollection<BasketItem>> GetBasketItemsAsync();

    public Task AddAsync(CatalogItemViewModel item);
}
