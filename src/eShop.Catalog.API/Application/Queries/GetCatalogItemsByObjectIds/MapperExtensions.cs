using eShop.Catalog.API.Model;
using eShop.Catalog.Contracts.GetCatalogItems;

namespace eShop.Catalog.API.Application.Queries.GetCatalogItemsByObjectIds;

internal static class MapperExtensions
{
    internal static CatalogItemDto[] MapToCatalogItemDtoList(this List<CatalogItem> catalogItems)
    {
        return catalogItems
            .Select(c => new CatalogItemDto(
                c.ObjectId,
                c.Name!,
                c.Description!,
                c.Price,
                c.PictureFileName!,
                new CatalogTypeDto(c.CatalogType!.ObjectId, c.CatalogType!.Type),
                new CatalogBrandDto(c.CatalogBrand!.ObjectId, c.CatalogBrand!.Brand),
                c.AvailableStock,
                c.RestockThreshold,
                c.MaxStockThreshold,
                c.OnReorder))
            .ToArray();
    }
}
