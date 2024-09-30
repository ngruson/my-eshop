namespace eShop.Catalog.Contracts.GetCatalogItems;

public record GetCatalogItemsResponse(
    int PageIndex,
    int PageSize,
    int Count,
    CatalogItemDto[] Data);
