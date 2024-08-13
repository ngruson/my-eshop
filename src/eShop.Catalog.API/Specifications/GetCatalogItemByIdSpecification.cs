using Ardalis.Specification;

namespace Catalog.API.Specifications;

public class GetCatalogItemByIdSpecification : Specification<CatalogItem>, ISingleResultSpecification<CatalogItem>
{
    public GetCatalogItemByIdSpecification(int id)
    {
        this.Query
            .Include(ci => ci.CatalogBrand)
            .Where(ci => ci.Id == id);
    }
}
