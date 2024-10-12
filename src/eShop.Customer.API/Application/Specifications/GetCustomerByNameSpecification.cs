using Ardalis.Specification;

namespace eShop.Customer.API.Application.Specifications;

internal class GetCustomerByNameSpecification : Specification<Domain.AggregatesModel.CustomerAggregate.Customer>
{
    public GetCustomerByNameSpecification(string name)
    {
        string firstName = name.Split(' ')[0];
        string lastName = name.Split(' ')[1];

        this.Query.Where(_ => _.FirstName == firstName && _.LastName == lastName && !_.IsDeleted);
    }
}
