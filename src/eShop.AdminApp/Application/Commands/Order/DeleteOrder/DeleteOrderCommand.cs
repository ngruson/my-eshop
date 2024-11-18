using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Order.DeleteOrder;

internal record DeleteOrderCommand(Guid ObjectId) : IRequest<Result>;
