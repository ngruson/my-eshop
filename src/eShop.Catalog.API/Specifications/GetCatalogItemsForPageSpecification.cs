using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsForPageSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsForPageSpecification(int pageSize, int pageIndex)
    {
        this.Query
            .Include(_ => _.CatalogBrand)
            .Include(_ => _.CatalogType)
            .Where(ci => ci.IsDeleted == false)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }
}
