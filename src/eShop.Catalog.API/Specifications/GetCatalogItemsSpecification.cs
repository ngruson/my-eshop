using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsSpecification(bool includeDeleted)
    {
        this.Query
            .Include(c => c.CatalogBrand)
            .Include(c => c.CatalogType)
            .Where(_ => includeDeleted || (!includeDeleted && !_.IsDeleted))
            .OrderBy(c => c.Name);
    }
}
