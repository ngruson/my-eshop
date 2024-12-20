
namespace eShop.Ordering.Domain.Events;

/// <summary>
/// Event used when an order is created
/// </summary>
public record class OrderStartedDomainEvent(
    Order Order, 
    Guid UserId,
    string UserName,
    string BuyerName,
    CardType CardType,
    string CardNumber,
    string CardSecurityNumber,
    string CardHolderName,
    DateTime CardExpiration) : INotification;
