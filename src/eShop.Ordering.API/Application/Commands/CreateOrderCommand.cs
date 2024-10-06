namespace eShop.Ordering.API.Application.Commands;

using eShop.Ordering.Contracts.CreateOrder;

public record CreateOrderCommand(
    OrderItemDto[] OrderItems,
    string UserId,
    string UserName,
    string City,
    string Street,
    string State,
    string Country,
    string ZipCode,
    string CardNumber,
    string CardHolderName,
    DateTime CardExpiration,
    string CardSecurityNumber,
    string CardType) : IRequest<bool>;
