using eShop.Shared.Data;

namespace eShop.ServiceInvocation.CatalogService;

internal static class MapperExtensions
{
    internal static PaginatedItems<CatalogItemViewModel> Map(this PaginatedItems<Catalog.Contracts.GetCatalogItems.CatalogItemDto> paginatedItems)
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
                _.PictureUrl,
                new CatalogTypeViewModel(_.CatalogType.ObjectId, _.CatalogType.Name),
                new CatalogBrandViewModel(_.CatalogBrand.ObjectId, _.CatalogBrand.Name),
                _.AvailableStock,
                _.RestockThreshold,
                _.MaxStockThreshold,
                _.OnReorder)).ToArray());
    }

    internal static CatalogItemViewModel[] Map(this Catalog.Contracts.GetCatalogItems.CatalogItemDto[] catalogItems)
    {
        return catalogItems.Select(_ => new CatalogItemViewModel(
            _.ObjectId,
            _.Name,
            _.Description,
            _.Price,
            _.PictureUrl,
            new CatalogTypeViewModel(_.CatalogType.ObjectId, _.CatalogType.Name),
            new CatalogBrandViewModel(_.CatalogBrand.ObjectId, _.CatalogBrand.Name),
            _.AvailableStock,
            _.RestockThreshold,
            _.MaxStockThreshold,
            _.OnReorder)).ToArray();
    }

    internal static CatalogItemViewModel Map(this Catalog.Contracts.GetCatalogItem.CatalogItemDto catalogItem)
    {
        return new CatalogItemViewModel(
            catalogItem.ObjectId,
            catalogItem.Name,
            catalogItem.Description,
            catalogItem.Price,
            catalogItem.PictureFileName,
            new CatalogTypeViewModel(catalogItem.CatalogType.ObjectId, catalogItem.CatalogType.Name),
            new CatalogBrandViewModel(catalogItem.CatalogBrand.ObjectId, catalogItem.CatalogBrand.Name),
            catalogItem.AvailableStock,
            catalogItem.RestockThreshold,
            catalogItem.MaxStockThreshold,
            catalogItem.OnReorder);
    }
}
