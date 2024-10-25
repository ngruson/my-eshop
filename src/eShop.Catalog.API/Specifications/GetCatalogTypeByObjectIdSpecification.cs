using Ardalis.Specification;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogTypeByObjectIdSpecification : Specification<CatalogType>, ISingleResultSpecification<CatalogType>
{
    public GetCatalogTypeByObjectIdSpecification(Guid objectId)
    {
        this.Query
            .Where(ci => ci.ObjectId == objectId);
    }
}
