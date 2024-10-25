namespace eShop.Catalog.Contracts.CreateCatalogItem;

public record CatalogItemDto(
    Guid ObjectId,
    string Name,
    string Description,
    decimal Price,
    string PictureFileName,
    Guid CatalogBrand,
    Guid CatalogType,
    int AvailableStock,
    int RestockThreshold,
    int MaxStockThreshold,
    bool OnReorder
);
