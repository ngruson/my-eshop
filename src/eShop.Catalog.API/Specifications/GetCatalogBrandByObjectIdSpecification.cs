using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogBrandByObjectIdSpecification : Specification<CatalogBrand>, ISingleResultSpecification<CatalogBrand>
{
    public GetCatalogBrandByObjectIdSpecification(Guid objectId)
    {
        this.Query
            .Where(ci => ci.ObjectId == objectId);
    }
}
