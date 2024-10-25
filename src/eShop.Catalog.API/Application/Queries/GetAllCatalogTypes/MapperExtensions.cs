using eShop.Catalog.Contracts.GetCatalogTypes;

namespace eShop.Catalog.API.Application.Queries.GetAllCatalogTypes;

internal static class MapperExtensions
{
    internal static CatalogTypeDto[] MapToCatalogTypeDtoList(this List<CatalogType> catalogTypes)
    {
        return catalogTypes
            .Select(c => new CatalogTypeDto(
                c.ObjectId,
                c.Type!))
            .ToArray();
    }
}
