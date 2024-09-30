using eShop.Catalog.Contracts.GetCatalogBrands;

namespace eShop.AdminApp.Application.Queries.GetCatalogBrands;

internal static class MapperExtensions
{
    public static CatalogBrandViewModel[] MapToCatalogBrandViewModelArray(this CatalogBrandDto[] catalogBrands)
    {
        return catalogBrands.Select(catalogBrand =>
            new CatalogBrandViewModel(
                catalogBrand.Id,
                catalogBrand.Brand)).ToArray();
    }
}
