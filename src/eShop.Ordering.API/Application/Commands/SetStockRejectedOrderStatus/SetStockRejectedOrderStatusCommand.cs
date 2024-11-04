namespace eShop.Ordering.API.Application.Commands.SetStockRejectedOrderStatus;

public record SetStockRejectedOrderStatusCommand(int OrderNumber, List<Guid> OrderStockItems) : IRequest<bool>;
