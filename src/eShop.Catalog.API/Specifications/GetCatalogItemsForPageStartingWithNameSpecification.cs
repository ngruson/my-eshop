using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsForPageStartingWithNameSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsForPageStartingWithNameSpecification(int pageSize, int pageIndex, string name)
    {
        this.Query.Where(c => c.Name.StartsWith(name))
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }
}
