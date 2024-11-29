namespace eShop.Ordering.Contracts.CreateOrder;

public record OrderDto(
    Guid UserId,
    string UserName,
    string BuyerName,
    string City,
    string Street,
    string State,
    string Country,
    string ZipCode,
    string CardNumber,
    string CardHolderName,
    DateTime CardExpiration,
    string CardSecurityNumber,
    Guid CardType,
    Guid Buyer,
    OrderItemDto[] Items);
