using Ardalis.Result;

namespace eShop.Ordering.API.Application.Commands.CancelOrder;

public record CancelOrderCommand(Guid ObjectId) : IRequest<Result>;

