using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsStartingWithNameSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsStartingWithNameSpecification(string name)
    {
        this.Query.Where(c => c.Name.StartsWith(name));
    }
}
