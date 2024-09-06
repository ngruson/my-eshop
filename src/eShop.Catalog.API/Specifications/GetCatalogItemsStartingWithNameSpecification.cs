using Ardalis.Specification;

namespace Catalog.API.Specifications;

public class GetCatalogItemsStartingWithNameSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsStartingWithNameSpecification(string name)
    {
        this.Query.Where(c => c.Name.StartsWith(name));
    }
}
