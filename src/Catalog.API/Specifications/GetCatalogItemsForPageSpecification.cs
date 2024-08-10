using Ardalis.Specification;

namespace Catalog.API.Specifications;

public class GetCatalogItemsForPageSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsForPageSpecification(int pageSize, int pageIndex)
    {
        this.Query
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }       
}
