using eShop.Catalog.Contracts.GetCatalogTypes;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogTypes;

internal static class MapperExtensions
{
    public static CatalogTypeViewModel[] MapToCatalogTypeViewModelArray(this CatalogTypeDto[] catalogTypes)
    {
        return catalogTypes.Select(catalogType =>
            new CatalogTypeViewModel(
                catalogType.ObjectId.ToString(),
                catalogType.Name)).ToArray();
    }
}
