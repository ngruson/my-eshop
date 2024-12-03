using Ardalis.Specification;
using eShop.Ordering.Domain.AggregatesModel.SalesTaxRateAggregate;

namespace eShop.Ordering.API.Application.Specifications;

public class SalesTaxRateSpecification : Specification<SalesTaxRate>, ISingleResultSpecification<SalesTaxRate>
{
    public SalesTaxRateSpecification(string state)
    {
        this.Query.Where(_ => _.State == state);
    }
}
