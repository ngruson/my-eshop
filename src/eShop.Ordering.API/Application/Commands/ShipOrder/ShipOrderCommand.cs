using Ardalis.Result;

namespace eShop.Ordering.API.Application.Commands.ShipOrder;

public record ShipOrderCommand(Guid ObjectId) : IRequest<Result>;
