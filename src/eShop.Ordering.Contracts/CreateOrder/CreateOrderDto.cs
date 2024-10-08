namespace eShop.Ordering.Contracts.CreateOrder;

public record CreateOrderDto(
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
    string CardType,
    string Buyer,
    OrderItemDto[] Items);
