using eShop.Catalog.API.Model;
using eShop.Catalog.Contracts.GetCatalogItem;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemByObjectId;

internal static class MapperExtensions
{
    internal static CatalogItemDto MapToCatalogItemDto(this CatalogItem catalogItem)
    {
        return new CatalogItemDto(
            catalogItem.ObjectId,
            catalogItem.Name!,
            catalogItem.Description!,
            catalogItem.Price,
            catalogItem.PictureFileName!,
            new CatalogTypeDto(catalogItem.CatalogType!.ObjectId, catalogItem.CatalogType!.Type),
            new CatalogBrandDto(catalogItem.CatalogBrand!.ObjectId, catalogItem.CatalogBrand!.Brand),
            catalogItem.AvailableStock,
            catalogItem.RestockThreshold,
            catalogItem.MaxStockThreshold,
            catalogItem.OnReorder);
    }
}
