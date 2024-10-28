using eShop.WebAppComponents.Services.ViewModels;

namespace eShop.WebAppComponents.Services;

public interface IProductImageUrlProvider
{
    string GetProductImageUrl(CatalogItemViewModel item)
        => this.GetProductImageUrl(item.ObjectId);

    string GetProductImageUrl(Guid productId);
}
