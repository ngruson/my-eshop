using eShop.WebAppComponents.Catalog;

namespace eShop.WebAppComponents.Services;

public interface IProductImageUrlProvider
{
    string GetProductImageUrl(CatalogItem item)
        => this.GetProductImageUrl(item.Id);

    string GetProductImageUrl(int productId);
}
