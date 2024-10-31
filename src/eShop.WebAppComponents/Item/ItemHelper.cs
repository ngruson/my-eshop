using eShop.ServiceInvocation.CatalogApiClient;

namespace eShop.WebAppComponents.Item;

public static class ItemHelper
{
    public static string Url(CatalogItemViewModel item)
        => $"item/{item.ObjectId}";
}
