using Ardalis.Result;
using eShop.Ordering.Contracts.UpdateOrder;

namespace eShop.Ordering.API.Application.Commands.UpdateOrder;

internal record UpdateOrderCommand(Guid ObjectId, OrderDto Dto) : IRequest<Result>;
