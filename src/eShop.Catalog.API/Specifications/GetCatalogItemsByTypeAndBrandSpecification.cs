using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsByTypeAndBrandSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsByTypeAndBrandSpecification(Guid type, Guid? brand)
    {
        this.Query.Include(_ => _.CatalogType);
        this.Query.Include(_ => _.CatalogBrand);

        if (brand is not null)
        {
            this.Query.Where(c => c.CatalogType!.ObjectId == type && c.CatalogBrand!.ObjectId == brand);
        }
        else
        {
            this.Query.Where(c => c.CatalogType!.ObjectId == type);
        }
    }
}
