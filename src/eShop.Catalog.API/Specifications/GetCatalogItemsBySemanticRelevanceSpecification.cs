using Ardalis.Specification;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace eShop.Catalog.API.Specifications;

public class GetCatalogItemsBySemanticRelevanceSpecification : Specification<CatalogItem>
{
    public GetCatalogItemsBySemanticRelevanceSpecification(Vector vector, int pageSize, int pageIndex)
    {
        this.Query
            .OrderBy(c => c.Embedding!.CosineDistance(vector))
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }
}
