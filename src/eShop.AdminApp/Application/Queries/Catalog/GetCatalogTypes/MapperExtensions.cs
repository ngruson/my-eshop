using eShop.Catalog.Contracts.GetCatalogTypes;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogTypes;

internal static class MapperExtensions
{
    public static CatalogTypeViewModel[] MapToCatalogTypeViewModelArray(this CatalogTypeDto[] catalogTypes)
    {
        return catalogTypes.Select(catalogType =>
            new CatalogTypeViewModel(
                catalogType.Id,
                catalogType.Type)).ToArray();
    }
}
