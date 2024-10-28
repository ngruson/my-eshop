using eShop.Shared.Data;
using eShop.WebAppComponents.Services.ViewModels;

namespace eShop.WebAppComponents.Services;

internal static class MapperExtensions
{
    internal static PaginatedItems<CatalogItemViewModel> Map(this PaginatedItems<eShop.Catalog.Contracts.GetCatalogItems.CatalogItemDto> paginatedItems)
    {
        return new PaginatedItems<CatalogItemViewModel>(
            paginatedItems.PageIndex,
            paginatedItems.PageSize,
            paginatedItems.Count,
            paginatedItems.Data.Select(_ => new CatalogItemViewModel(
                _.ObjectId,
                _.Name,
                _.Description,
                _.Price,
                _.PictureFileName,
                new CatalogTypeViewModel(_.CatalogType.ObjectId, _.CatalogType.Name),
                new CatalogBrandViewModel(_.CatalogBrand.ObjectId, _.CatalogBrand.Name)))
            .ToArray());
    }

    internal static CatalogItemViewModel[] Map(this eShop.Catalog.Contracts.GetCatalogItems.CatalogItemDto[] catalogItems)
    {
        return catalogItems.Select(_ => new CatalogItemViewModel(
            _.ObjectId,
            _.Name,
            _.Description,
            _.Price,
            _.PictureFileName,
            new CatalogTypeViewModel(_.CatalogType.ObjectId, _.CatalogType.Name),
            new CatalogBrandViewModel(_.CatalogBrand.ObjectId, _.CatalogBrand.Name)))
            .ToArray();
    }

    internal static CatalogItemViewModel Map(this eShop.Catalog.Contracts.GetCatalogItem.CatalogItemDto catalogItem)
    {
        return new CatalogItemViewModel(
            catalogItem.ObjectId,
            catalogItem.Name,
            catalogItem.Description,
            catalogItem.Price,
            catalogItem.PictureFileName,
            new CatalogTypeViewModel(catalogItem.CatalogType.ObjectId, catalogItem.CatalogType.Name),
            new CatalogBrandViewModel(catalogItem.CatalogBrand.ObjectId, catalogItem.CatalogBrand.Name));
    }
}
