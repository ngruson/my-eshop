using eShop.ServiceInvocation.CatalogService;

namespace eShop.WebApp.Services;

public interface IBasketState
{
    public Task<IReadOnlyCollection<BasketItem>> GetBasketItemsAsync();

    public Task AddAsync(CatalogItemViewModel item);
}
