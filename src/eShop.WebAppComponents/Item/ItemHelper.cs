using eShop.WebAppComponents.Services.ViewModels;

namespace eShop.WebAppComponents.Item;

public static class ItemHelper
{
    public static string Url(CatalogItemViewModel item)
        => $"item/{item.ObjectId}";
}
