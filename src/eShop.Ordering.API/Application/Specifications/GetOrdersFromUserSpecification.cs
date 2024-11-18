using Ardalis.Specification;
using OrderAggregate = eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.API.Application.Specifications;
public class GetOrdersFromUserSpecification : Specification<OrderAggregate.Order>
{
    public GetOrdersFromUserSpecification(Guid userId)
    {
        this.Query
            .Include(_ => _.OrderItems)
            .Where(_ => _.Buyer!.IdentityGuid == userId);
    }
}
