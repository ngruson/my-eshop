using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsForPageByBrandSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsForPageByBrandSpecification(Guid brand, int pageSize, int pageIndex)
    {
        this.Query.Include(ci => ci.CatalogBrand);
        this.Query.Include(ci => ci.CatalogType);
        this.Query
            .Where(ci => ci.CatalogBrand!.ObjectId == brand)
            .Skip(pageSize * pageIndex)
            .Take(pageSize);  
    }
}
