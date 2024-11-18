using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Order.GetOrder;

internal record GetOrderQuery(Guid ObjectId) : IRequest<Result<OrderViewModel>>;
