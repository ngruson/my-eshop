using Ardalis.Specification;

namespace eShop.Ordering.API.Application.Specifications
{
    public class GetOrdersSpecification : Specification<Domain.AggregatesModel.OrderAggregate.Order>
    {
        public GetOrdersSpecification()
        {
            this.Query.Include(_ => _.Buyer);
            this.Query.Include(_ => _.OrderItems);
        }
    }
}
