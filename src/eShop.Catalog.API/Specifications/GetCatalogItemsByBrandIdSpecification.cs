using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsByBrandIdSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsByBrandIdSpecification(int? brandId)
    {
        this.Query.Where(_ => _.CatalogBrandId == brandId);
    }
}
