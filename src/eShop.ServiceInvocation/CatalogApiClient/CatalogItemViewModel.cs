namespace eShop.ServiceInvocation.CatalogApiClient;

public record CatalogItemViewModel(
    Guid ObjectId,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    CatalogTypeViewModel CatalogType,
    CatalogBrandViewModel CatalogBrand,
    int AvailableStock,
    int RestockThreshold,
    int MaxStockThreshold,
    bool OnReorder
);
