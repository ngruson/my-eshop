using eShop.WebAppComponents.Catalog;

namespace eShop.WebAppComponents.Services
{
    public interface ICatalogService
    {
        Task<CatalogItem?> GetCatalogItem(int id);
        Task<CatalogItem[]> GetCatalogItems();
        Task<CatalogResult> GetPaginatedCatalogItems(int pageIndex, int pageSize, int? brand, int? type);
        Task<CatalogItem[]> GetCatalogItems(IEnumerable<int> ids);
        Task<CatalogResult> GetCatalogItemsWithSemanticRelevance(int page, int take, string text);
        Task<CatalogBrand[]> GetBrands();
        Task<CatalogItemType[]> GetTypes();
    }
}
