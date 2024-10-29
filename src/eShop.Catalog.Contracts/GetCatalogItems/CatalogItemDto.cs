namespace eShop.Catalog.Contracts.GetCatalogItems;

public record CatalogItemDto(
    Guid ObjectId,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    CatalogTypeDto CatalogType,
    CatalogBrandDto CatalogBrand,
    int AvailableStock,
    int RestockThreshold,
    int MaxStockThreshold,
    bool OnReorder
);
