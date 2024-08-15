namespace eShop.Catalog.API.Extensions;

public static class MappingExtensions
{
    public static void Map(this CatalogItem catalogItem, CatalogItem update)
    {
        catalogItem.Name = update.Name;
        catalogItem.Description = update.Description;
        catalogItem.Price = update.Price;
        catalogItem.PictureFileName = update.PictureFileName;
        catalogItem.CatalogTypeId = update.CatalogTypeId;
        catalogItem.CatalogBrandId = update.CatalogBrandId;
        catalogItem.AvailableStock = update.AvailableStock;
        catalogItem.RestockThreshold = update.RestockThreshold;
        catalogItem.MaxStockThreshold = update.MaxStockThreshold;
        catalogItem.OnReorder = update.OnReorder;
    }
}
