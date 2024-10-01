using eShop.Catalog.Contracts.GetCatalogBrands;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Catalog.Contracts.GetCatalogTypes;
using Refit;

namespace eShop.Catalog.Contracts;

public interface ICatalogApi
{
    [Get("/api/catalog/items?api-version=1.0")]
    Task<CatalogItemDto[]> GetCatalogItems();

    [Get("/api/catalog/catalogTypes?api-version=1.0")]
    Task<CatalogTypeDto[]> GetCatalogTypes();

    [Get("/api/catalog/catalogBrands?api-version=1.0")]
    Task<CatalogBrandDto[]> GetCatalogBrands();
}
