using Ardalis.Specification;

namespace eShop.Customer.API.Application.Specifications;

internal class GetCustomerSpecification : Specification<Domain.AggregatesModel.CustomerAggregate.Customer>
{
    public GetCustomerSpecification(string firstName, string lastName)
    {
        this.Query.Where(_ => _.FirstName == firstName && _.LastName == lastName);
    }
}
