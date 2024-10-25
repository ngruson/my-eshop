using eShop.Catalog.Contracts.GetCatalogBrands;

namespace eShop.Catalog.API.Application.Queries.GetAllCatalogBrands;

internal static class MapperExtensions
{
    internal static CatalogBrandDto[] MapToCatalogBrandDtoList(this List<CatalogBrand> catalogBrands)
    {
        return catalogBrands
            .Select(c => new CatalogBrandDto(
                c.ObjectId,
                c.Brand!))
            .ToArray();
    }
}
