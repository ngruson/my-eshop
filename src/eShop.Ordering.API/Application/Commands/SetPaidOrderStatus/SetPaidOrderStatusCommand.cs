using Ardalis.Result;

namespace eShop.Ordering.API.Application.Commands.SetPaidOrderStatus;

public record SetPaidOrderStatusCommand(Guid OrderId) : IRequest<Result>;
