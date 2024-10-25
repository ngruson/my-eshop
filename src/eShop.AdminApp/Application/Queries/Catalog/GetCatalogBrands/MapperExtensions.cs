using eShop.Catalog.Contracts.GetCatalogBrands;

namespace eShop.AdminApp.Application.Queries.Catalog.GetCatalogBrands;

internal static class MapperExtensions
{
    public static CatalogBrandViewModel[] MapToCatalogBrandViewModelArray(this CatalogBrandDto[] catalogBrands)
    {
        return catalogBrands.Select(catalogBrand =>
            new CatalogBrandViewModel(
                catalogBrand.ObjectId.ToString(),
                catalogBrand.Name)).ToArray();
    }
}
