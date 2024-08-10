using Ardalis.Specification;

namespace Catalog.API.Specifications;

public class GetCatalogItemsForPageByBrandIdSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsForPageByBrandIdSpecification(int? brandId, int pageSize, int pageIndex)
    {
        if (brandId is not null)
        {
            this.Query.Where(ci => ci.CatalogBrandId == brandId);
        }

        this.Query
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }
}
