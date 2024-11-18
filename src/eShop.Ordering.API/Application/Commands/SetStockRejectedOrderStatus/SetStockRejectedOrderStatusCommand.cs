namespace eShop.Ordering.API.Application.Commands.SetStockRejectedOrderStatus;

public record SetStockRejectedOrderStatusCommand(Guid ObjectId, List<Guid> OrderStockItems) : IRequest<bool>;
