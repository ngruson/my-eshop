using Ardalis.Result;

namespace eShop.Ordering.API.Application.Commands;

public record SetAwaitingValidationOrderStatusCommand(int OrderNumber) : IRequest<Result>;
