using eShop.Catalog.Contracts.GetCatalogItems;
using Refit;

namespace eShop.Catalog.Contracts;

public interface ICatalogApi
{
    [Get("/api/catalog/items?api-version=1.0")]
    Task<GetCatalogItemsResponse> GetCatalogItems();
}
