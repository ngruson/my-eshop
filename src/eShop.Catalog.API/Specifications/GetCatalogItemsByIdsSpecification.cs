using Ardalis.Specification;

namespace Catalog.API.Specifications;

public class GetCatalogItemsByIdsSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsByIdsSpecification(int[] ids)
    {
        this.Query.Where(item => ids.Contains(item.Id));
    }
}
