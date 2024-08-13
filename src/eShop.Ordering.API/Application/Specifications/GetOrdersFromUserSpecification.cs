using Ardalis.Specification;
using OrderAggregate = eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.API.Application.Specifications;
public class GetOrdersFromUserSpecification : Specification<OrderAggregate.Order>
{
    public GetOrdersFromUserSpecification(string userId)
    {
        this.Query.Where(_ => _.Buyer.IdentityGuid == userId);
    }
}
