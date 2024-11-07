using Ardalis.Result;
using eShop.Ordering.Contracts.GetOrdersFromUser;

namespace eShop.Ordering.API.Application.Queries.GetOrdersFromUser;

internal record GetOrdersFromUserQuery : IRequest<Result<OrderDto[]>>;
