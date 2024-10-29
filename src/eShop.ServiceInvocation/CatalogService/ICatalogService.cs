using eShop.Catalog.Contracts.GetCatalogBrands;
using eShop.Catalog.Contracts.GetCatalogTypes;
using eShop.Shared.Data;

namespace eShop.ServiceInvocation.CatalogService;

public interface ICatalogService
{
    Task CreateCatalogItem(Catalog.Contracts.CreateCatalogItem.CreateCatalogItemDto dto);
    Task DeleteCatalogItem(Guid objectId);
    Task<CatalogItemViewModel> GetCatalogItem(Guid objectId);
    Task<Catalog.Contracts.GetCatalogItems.CatalogItemDto[]> GetCatalogItems(bool includeDeleted = false);
    Task<PaginatedItems<CatalogItemViewModel>> GetPaginatedCatalogItems(Guid? catalogType, Guid? catalogBrand, int pageSize, int pageIndex);
    Task<CatalogItemViewModel[]> GetCatalogItems(Guid[] ids);
    Task<PaginatedItems<CatalogItemViewModel>> GetPaginatedCatalogItemsWithSemanticRelevance(string text, int pageSize, int pageIndex);
    Task<CatalogBrandDto[]> GetBrands();
    Task<CatalogTypeDto[]> GetTypes();
    Task UpdateCatalogItem(Guid objectId, Catalog.Contracts.UpdateCatalogItem.CatalogItemDto dto);
}
