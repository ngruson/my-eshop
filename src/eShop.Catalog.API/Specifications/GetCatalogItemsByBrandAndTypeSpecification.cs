using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsByBrandAndTypeSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsByBrandAndTypeSpecification(int typeId, int? brandId)
    {
        if (brandId is not null)
        {
            this.Query.Where(c => c.CatalogTypeId == typeId && c.CatalogBrandId == brandId);
        }
        else
        {
            this.Query.Where(c => c.CatalogTypeId == typeId);
        }
    }
}
