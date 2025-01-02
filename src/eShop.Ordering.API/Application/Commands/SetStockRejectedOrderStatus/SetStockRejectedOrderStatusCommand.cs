using Ardalis.Result;

namespace eShop.Ordering.API.Application.Commands.SetStockRejectedOrderStatus;

public record SetStockRejectedOrderStatusCommand(Guid ObjectId, Guid[] OrderStockItems) : IRequest<Result>;
