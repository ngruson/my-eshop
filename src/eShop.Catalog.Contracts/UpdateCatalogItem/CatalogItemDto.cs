namespace eShop.Catalog.Contracts.UpdateCatalogItem;

public record CatalogItemDto(
    string Name,
    string Description,
    decimal Price,
    string PictureFileName,
    Guid CatalogType,
    Guid CatalogBrand,
    int AvailableStock,
    int RestockThreshold,
    int MaxStockThreshold,
    bool OnReorder);
