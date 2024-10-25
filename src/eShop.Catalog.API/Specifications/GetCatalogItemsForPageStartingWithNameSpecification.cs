using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsForPageStartingWithNameSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsForPageStartingWithNameSpecification(string name, int pageSize, int pageIndex)
    {
        this.Query.Include(_ => _.CatalogType);
        this.Query.Include(_ => _.CatalogBrand);
        this.Query.Where(c => c.Name!.StartsWith(name))
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }
}
