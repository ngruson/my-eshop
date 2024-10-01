using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsSpecification()
    {
        this.Query
            .OrderBy(c => c.Name);
    }
}
