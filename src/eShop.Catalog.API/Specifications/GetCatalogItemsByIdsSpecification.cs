using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsByIdsSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsByIdsSpecification(Guid[] ids)
    {
        this.Query.Include(_ => _.CatalogType);
        this.Query.Include(_ => _.CatalogBrand);
        this.Query.Where(item => ids.Contains(item.ObjectId));
    }
}
