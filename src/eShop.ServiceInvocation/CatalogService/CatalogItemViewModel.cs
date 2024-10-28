namespace eShop.ServiceInvocation.CatalogService;

public record CatalogItemViewModel(
    Guid ObjectId,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    CatalogTypeViewModel CatalogType,
    CatalogBrandViewModel CatalogBrand
);
