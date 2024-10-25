using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsByBrandSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsByBrandSpecification(Guid brand)
    {
        this.Query.Include(_ => _.CatalogBrand);
        this.Query.Where(_ => _.CatalogBrand!.ObjectId == brand);
    }
}
