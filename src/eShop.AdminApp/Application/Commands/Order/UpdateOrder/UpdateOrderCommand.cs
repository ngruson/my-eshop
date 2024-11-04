using Ardalis.Result;
using eShop.Ordering.Contracts.UpdateOrder;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Order.UpdateOrder;

internal record UpdateOrderCommand(Guid ObjectId, OrderDto Dto) : IRequest<Result>;
