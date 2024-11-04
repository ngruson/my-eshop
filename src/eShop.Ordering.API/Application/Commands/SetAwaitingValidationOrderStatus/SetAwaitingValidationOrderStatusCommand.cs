namespace eShop.Ordering.API.Application.Commands.SetAwaitingValidationOrderStatus;

public record SetAwaitingValidationOrderStatusCommand(Guid OrderId) : IRequest<bool>;
