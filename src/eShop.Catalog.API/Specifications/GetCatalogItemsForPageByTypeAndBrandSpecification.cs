using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsForPageByTypeAndBrandSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsForPageByTypeAndBrandSpecification(Guid type, Guid? brand, int pageSize, int pageIndex)
    {
        this.Query.Include(ci => ci.CatalogType);
        this.Query.Include(ci => ci.CatalogBrand);

        if (brand is not null)
        {
            this.Query.Where(c => c.CatalogType!.ObjectId == type && c.CatalogBrand!.ObjectId == brand);
        }
        else
        {
            this.Query.Where(c => c.CatalogType!.ObjectId == type);
        }

        this.Query
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }
}
