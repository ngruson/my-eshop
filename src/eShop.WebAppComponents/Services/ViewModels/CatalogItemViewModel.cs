namespace eShop.WebAppComponents.Services.ViewModels;

public record CatalogItemViewModel(
    Guid ObjectId,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    CatalogTypeViewModel CatalogType,
    CatalogBrandViewModel CatalogBrand
);
