using eShop.Catalog.Contracts.GetCatalogBrands;
using eShop.Catalog.Contracts.GetCatalogTypes;
using eShop.Shared.Data;

namespace eShop.ServiceInvocation.CatalogService;

public interface ICatalogService
{
    Task<CatalogItemViewModel> GetCatalogItem(Guid objectId);
    Task<Catalog.Contracts.GetCatalogItems.CatalogItemDto[]> GetCatalogItems();
    Task<PaginatedItems<CatalogItemViewModel>> GetPaginatedCatalogItems(Guid? catalogType, Guid? catalogBrand, int pageIndex, int pageSize);
    Task<CatalogItemViewModel[]> GetCatalogItems(Guid[] ids);
    Task<PaginatedItems<CatalogItemViewModel>> GetPaginatedCatalogItemsWithSemanticRelevance(string text, int pageSize, int pageIndex);
    Task<CatalogBrandDto[]> GetBrands();
    Task<CatalogTypeDto[]> GetTypes();
}
