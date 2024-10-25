using eShop.Catalog.Contracts.GetCatalogItems;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogItems;

internal static class MapperExtensions
{
    public static CatalogItemViewModel[] MapToCatalogItemViewModelArray(this CatalogItemDto[] catalogItems)
    {
        return catalogItems.Select(catalogItem =>
            new CatalogItemViewModel(
                catalogItem.ObjectId,
                catalogItem.Name,
                catalogItem.Description,
                catalogItem.Price,
                new CatalogTypeViewModel(catalogItem.CatalogType.ObjectId, catalogItem.CatalogType.Name),
                new CatalogBrandViewModel(catalogItem.CatalogBrand.ObjectId, catalogItem.CatalogBrand.Name),
                catalogItem.AvailableStock,
                catalogItem.RestockThreshold,
                catalogItem.MaxStockThreshold,
                catalogItem.OnReorder)).ToArray();
    }
}
