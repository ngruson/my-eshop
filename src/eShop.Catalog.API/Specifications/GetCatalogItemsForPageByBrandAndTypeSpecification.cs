using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsForPageByBrandAndTypeSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsForPageByBrandAndTypeSpecification(int typeId, int? brandId, int pageSize, int pageIndex)
    {
        if (brandId is not null)
        {
            this.Query.Where(c => c.CatalogTypeId == typeId && c.CatalogBrandId == brandId);
        }
        else
        {
            this.Query.Where(c => c.CatalogTypeId == typeId);
        }

        this.Query
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }
}
