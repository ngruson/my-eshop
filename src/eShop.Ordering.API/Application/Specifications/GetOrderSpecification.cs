using Ardalis.Specification;

namespace eShop.Ordering.API.Application.Specifications;
public class GetOrderSpecification : Specification<Order>, ISingleResultSpecification<Order>
{
    public GetOrderSpecification(Guid objectId)
    {
        this.Query
            .Include(_ => _.Buyer)
            .Include(_ => _.OrderItems)
            .Where(_ => _.ObjectId == objectId);
    }
}
