using Ardalis.Specification;
using OrderAggregate = eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.API.Application.Specifications;
public class GetOrderSpecification : Specification<OrderAggregate.Order>, ISingleResultSpecification<OrderAggregate.Order>
{
    public GetOrderSpecification(Guid objectId)
    {
        this.Query
            .Include(_ => _.Buyer)
            .Include(_ => _.OrderItems)
            .Where(_ => _.ObjectId == objectId);
    }
}
