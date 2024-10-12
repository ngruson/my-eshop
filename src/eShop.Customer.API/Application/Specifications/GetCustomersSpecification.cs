using Ardalis.Specification;

namespace eShop.Customer.API.Application.Specifications;

internal class GetCustomersSpecification : Specification<Domain.AggregatesModel.CustomerAggregate.Customer>
{
    public GetCustomersSpecification(bool includeDeleted)
    {
        this.Query
            .Where(_ => includeDeleted || (!includeDeleted && !_.IsDeleted))
            .OrderBy(_ => _.FirstName)
            .ThenBy(_ => _.LastName);
    }
}
