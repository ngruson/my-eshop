namespace eShop.Ordering.API.Application.Commands.ShipOrder;

public record ShipOrderCommand(Guid ObjectId) : IRequest<bool>;
