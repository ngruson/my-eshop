using eShop.Catalog.Contracts.UpdateCatalogItem;
using Refit;

namespace eShop.Catalog.Contracts;

public interface ICatalogApi
{
    [Get("/api/catalog/items?api-version=1.0")]
    Task<GetCatalogItems.CatalogItemDto[]> GetCatalogItems(bool includeDeleted = false);

    [Get("/api/catalog/items/{objectId}?api-version=1.0")]
    Task<GetCatalogItem.CatalogItemDto> GetCatalogItem(Guid objectId);

    [Post("/api/catalog/items?api-version=1.0")]
    Task CreateCatalogItem(CreateCatalogItem.CreateCatalogItemDto dto);

    [Put("/api/catalog/items/{objectId}?api-version=1.0")]
    Task UpdateCatalogItem(Guid objectId, CatalogItemDto dto);

    [Delete("/api/catalog/items/{objectId}?api-version=1.0")]
    Task DeleteCatalogItem(Guid objectId);

    [Get("/api/catalog/catalogTypes?api-version=1.0")]
    Task<GetCatalogTypes.CatalogTypeDto[]> GetCatalogTypes();

    [Get("/api/catalog/catalogBrands?api-version=1.0")]
    Task<GetCatalogBrands.CatalogBrandDto[]> GetCatalogBrands();
}
