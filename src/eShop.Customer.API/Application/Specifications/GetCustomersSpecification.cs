using Ardalis.Specification;

namespace eShop.Customer.API.Application.Specifications;

internal class GetCustomersSpecification : Specification<Domain.AggregatesModel.CustomerAggregate.Customer>
{
    public GetCustomersSpecification()
    {
        this.Query
            .OrderBy(_ => _.FirstName)
            .ThenBy(_ => _.LastName);
    }
}
