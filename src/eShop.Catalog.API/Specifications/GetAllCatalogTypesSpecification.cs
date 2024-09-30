using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetAllCatalogTypesSpecification : Specification<CatalogType>
{
    public GetAllCatalogTypesSpecification()
    {
        this.Query.OrderBy(_ => _.Type);
    }
}
