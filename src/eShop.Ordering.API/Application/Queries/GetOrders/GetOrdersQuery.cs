using Ardalis.Result;
using eShop.Ordering.Contracts.GetOrders;

namespace eShop.Ordering.API.Application.Queries.GetOrders;

internal class GetOrdersQuery : IRequest<Result<List<OrderDto>>>
{
}
