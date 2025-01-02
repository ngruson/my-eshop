using eShop.Catalog.Contracts.AssessStockItemsForOrder;
using eShop.Catalog.Contracts.UpdateCatalogItem;
using eShop.Shared.Data;
using Refit;

namespace eShop.Catalog.Contracts;

public interface ICatalogApi
{
    [Get("/api/catalog/items?api-version=1.0")]
    Task<GetCatalogItems.CatalogItemDto[]> GetCatalogItems(bool includeDeleted = false);

    [Get("/api/catalog/items/page?pageSize={pageSize}&pageIndex={pageIndex}&api-version=1.0")]
    Task<PaginatedItems<GetCatalogItems.CatalogItemDto>> GetPaginatedCatalogItems(int pageSize, int pageIndex);

    [Get("/api/catalog/items/by/{name}?pageSize={pageSize}&pageIndex={pageIndex}&api-version=1.0")]
    Task<PaginatedItems<GetCatalogItems.CatalogItemDto>> GetPaginatedCatalogItemsByName(string name, int pageSize, int pageIndex);

    [Get("/api/catalog/items/withSemanticRelevance/{text}?pageSize={pageSize}&pageIndex={pageIndex}&api-version=1.0")]
    Task<PaginatedItems<GetCatalogItems.CatalogItemDto>> GetPaginatedCatalogItemsWithSemanticRelevance(string text, int pageSize, int pageIndex);

    [Get("/api/catalog/items/type/{catalogType}/brand/{catalogBrand}?pageSize={pageSize}&pageIndex={pageIndex}&api-version=1.0")]
    Task<PaginatedItems<GetCatalogItems.CatalogItemDto>> GetPaginatedCatalogItemsByTypeAndBrand(Guid catalogType, Guid? catalogBrand, int pageSize, int pageIndex);

    [Get("/api/catalog/items/type/all/brand/{catalogBrand}?pageSize={pageSize}&pageIndex={pageIndex}&api-version=1.0")]
    Task<PaginatedItems<GetCatalogItems.CatalogItemDto>> GetPaginatedCatalogItemsByBrand(Guid catalogBrand, int pageSize, int pageIndex);

    [Get("/api/catalog/items/by?api-version=1.0")]
    Task<GetCatalogItems.CatalogItemDto[]> GetCatalogItemsByIds([Query(CollectionFormat.Multi)] Guid[] ids);

    [Get("/api/catalog/items/{objectId}?api-version=1.0")]
    Task<GetCatalogItem.CatalogItemDto> GetCatalogItem(Guid objectId);

    [Get("/api/catalog/items/{objectId}/pic?api-version=1.0")]
    Task<byte[]> GetCatalogItemPicture(Guid objectId);

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

    [Post("/api/catalog/assessStockItemsForOrder?api-version=1.0")]
    Task<AssessStockItemsForOrderResponseDto> AssessStockItemsForOrder(AssessStockItemsForOrderRequestDto dto);
}
