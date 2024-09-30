using eShop.Catalog.Contracts.GetCatalogItems;

namespace eShop.AdminApp.Application.Queries.GetCatalogItems;

internal static class MapperExtensions
{
    public static CatalogItemViewModel[] MapToCatalogItemViewModelArray(this CatalogItemDto[] catalogItems)
    {
        return catalogItems.Select(catalogItem =>
        new CatalogItemViewModel(
            catalogItem.Id,
            catalogItem.Name,
            catalogItem.Description,
            catalogItem.Price)).ToArray();        
    }
}
