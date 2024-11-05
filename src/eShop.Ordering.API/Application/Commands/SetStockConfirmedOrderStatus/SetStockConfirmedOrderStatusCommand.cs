namespace eShop.Ordering.API.Application.Commands.SetStockConfirmedOrderStatus;

public record SetStockConfirmedOrderStatusCommand(Guid ObjectId) : IRequest<bool>;
