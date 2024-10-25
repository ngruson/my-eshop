using eShop.WebAppComponents.Services;

namespace eShop.WebApp.Services;

public class ProductImageUrlProvider : IProductImageUrlProvider
{
    public string GetProductImageUrl(Guid productId)
        => $"product-images/{productId}?api-version=1.0";
}
