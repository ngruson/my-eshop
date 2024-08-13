using Ardalis.Specification;

namespace Catalog.API.Specifications;

public class GetAllCatalogBrandsSpecification : Specification<CatalogBrand>
{
    public GetAllCatalogBrandsSpecification()
    {
        this.Query.OrderBy(_ => _.Brand);
    }
}
