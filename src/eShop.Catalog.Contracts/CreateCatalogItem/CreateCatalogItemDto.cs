namespace eShop.Catalog.Contracts.CreateCatalogItem;

public record CreateCatalogItemDto(
    string Name,
    string Description,
    decimal Price,
    string PictureFileName,
    Guid CatalogType,
    Guid CatalogBrand,
    int AvailableStock,
    int RestockThreshold,
    int MaxStockThreshold);
