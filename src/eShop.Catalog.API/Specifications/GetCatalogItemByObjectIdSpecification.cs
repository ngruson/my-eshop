using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemByObjectIdSpecification : Specification<CatalogItem>, ISingleResultSpecification<CatalogItem>
{
    public GetCatalogItemByObjectIdSpecification(Guid objectId)
    {
        this.Query
            .Include(ci => ci.CatalogBrand)
            .Include(ci => ci.CatalogType)
            .Where(ci => ci.ObjectId == objectId);
    }
}
