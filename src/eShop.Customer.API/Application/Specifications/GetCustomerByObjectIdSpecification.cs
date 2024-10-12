using Ardalis.Specification;

namespace eShop.Customer.API.Application.Specifications;

internal class GetCustomerByObjectIdSpecification : Specification<Domain.AggregatesModel.CustomerAggregate.Customer>
{
    public GetCustomerByObjectIdSpecification(Guid objectId)
    {
        this.Query.Where(_ => _.ObjectId == objectId && !_.IsDeleted);
    }
}
