using Ardalis.Result;

namespace eShop.Ordering.API.Application.Queries.GetOrders;

public class GetOrdersQuery : IRequest<Result<List<OrderDto>>>
{
}
