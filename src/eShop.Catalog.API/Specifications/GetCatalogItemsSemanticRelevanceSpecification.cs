using Ardalis.Specification;
using eShop.Catalog.API.Application.Queries.GetCatalogItemsBySemanticRelevance;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace eShop.Catalog.API.Specifications;

internal class GetCatalogItemsSemanticRelevanceSpecification : Specification<CatalogItem, CatalogItemSemanticRelevance>
{
    public GetCatalogItemsSemanticRelevanceSpecification(Vector vector, int pageSize, int pageIndex)
    {
        this.Query.Include(_ => _.CatalogType);
        this.Query.Include(_ => _.CatalogBrand);
        this.Query
            .Select(c => new CatalogItemSemanticRelevance(c.Name!, c.Embedding!.CosineDistance(vector)))
            .OrderBy(c => c.Embedding!.CosineDistance(vector))
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }
}
