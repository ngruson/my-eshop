using eShop.Catalog.Contracts.CreateCatalogItem;

namespace eShop.Catalog.API.Application.Commands.CreateCatalogItem;

internal static class MapperExtensions
{
    public static CatalogItem MapFromDto(this CreateCatalogItemDto dto, CatalogType catalogType, CatalogBrand? catalogBrand)
    {
        return new CatalogItem(
            Guid.NewGuid(),
            dto.Name,
            dto.Description,
            dto.Price,
            dto.PictureFileName,
            catalogType,
            catalogBrand,
            dto.AvailableStock,
            dto.RestockThreshold,
            dto.MaxStockThreshold);
    }

    internal static CatalogItemDto MapToDto(this CatalogItem catalogItem)
    {
        return new CatalogItemDto(
            catalogItem.ObjectId,
            catalogItem.Name!,
            catalogItem.Description!,
            catalogItem.Price,
            catalogItem.PictureFileName!,
            catalogItem.CatalogBrand!.ObjectId,
            catalogItem.CatalogType!.ObjectId,
            catalogItem.AvailableStock,
            catalogItem.RestockThreshold,
            catalogItem.MaxStockThreshold,
            catalogItem.OnReorder);
    }
}
