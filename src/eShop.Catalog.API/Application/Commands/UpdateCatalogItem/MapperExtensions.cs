using eShop.Catalog.Contracts.UpdateCatalogItem;

namespace eShop.Catalog.API.Application.Commands.UpdateCatalogItem;

internal static class MapperExtensions
{
    public static void MapFromDto(this CatalogItemDto dto, CatalogItem catalogItem, CatalogType catalogType, CatalogBrand catalogBrand)
    {
        catalogItem.Name = dto.Name;
        catalogItem.Description = dto.Description;
        catalogItem.Price = dto.Price;
        catalogItem.PictureFileName = dto.PictureFileName;
        catalogItem.CatalogType = catalogType;
        catalogItem.CatalogBrand = catalogBrand;
        catalogItem.AvailableStock = dto.AvailableStock;
        catalogItem.RestockThreshold = dto.RestockThreshold;
        catalogItem.MaxStockThreshold = dto.MaxStockThreshold;
    }
}
