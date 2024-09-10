using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.GetOrders;

internal class GetOrdersQuery : IRequest<Result<List<OrderViewModel>>>
{
}
