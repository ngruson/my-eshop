using eShop.AdminApp.Application.Commands.Catalog.CreateCatalogItem;
using eShop.AdminApp.Application.Commands.Catalog.UpdateCatalogItem;
using eShop.Catalog.Contracts.CreateCatalogItem;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogItem;

internal static class MapperExtensions
{
    internal static CatalogItemViewModel MapToCatalogItemViewModel(this ServiceInvocation.CatalogApiClient.CatalogItemViewModel catalogItem)
    {
        return new CatalogItemViewModel(
            catalogItem.ObjectId,
            catalogItem.Name,
            catalogItem.Description,
            catalogItem.Price,
            catalogItem.PictureUrl,
            catalogItem.CatalogType.ObjectId.ToString(),
            catalogItem.CatalogBrand.ObjectId.ToString(),
            catalogItem.AvailableStock,
            catalogItem.RestockThreshold,
            catalogItem.MaxStockThreshold,
            catalogItem.OnReorder);
    }

    internal static CreateCatalogItemCommand MapToCreateCatalogItemCommand(this CatalogItemViewModel catalogItem)
    {
        return new CreateCatalogItemCommand(
            new CreateCatalogItemDto(
                catalogItem.Name,
                catalogItem.Description,
                catalogItem.Price,
                catalogItem.PictureFileName,
                Guid.Parse(catalogItem.CatalogType),
                Guid.Parse(catalogItem.CatalogBrand),
                catalogItem.AvailableStock,
                catalogItem.RestockThreshold,
                catalogItem.MaxStockThreshold));
    }

    internal static UpdateCatalogItemCommand MapToUpdateCatalogItemCommand(this CatalogItemViewModel catalogItem)
    {
        return new UpdateCatalogItemCommand(
            catalogItem.ObjectId,
            new eShop.Catalog.Contracts.UpdateCatalogItem.CatalogItemDto(
                catalogItem.Name,
                catalogItem.Description,
                catalogItem.Price,
                catalogItem.PictureFileName,
                Guid.Parse(catalogItem.CatalogType),
                Guid.Parse(catalogItem.CatalogBrand),
                catalogItem.AvailableStock,
                catalogItem.RestockThreshold,
                catalogItem.MaxStockThreshold,
                catalogItem.OnReorder));
    }
}
