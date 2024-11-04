using Ardalis.Result;
using eShop.Ordering.Contracts.GetOrder;

namespace eShop.Ordering.API.Application.Queries.GetOrder;

internal record GetOrderQuery(Guid ObjectId) : IRequest<Result<OrderDto>>;
