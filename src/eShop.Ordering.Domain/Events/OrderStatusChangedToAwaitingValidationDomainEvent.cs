namespace eShop.Ordering.Domain.Events;

/// <summary>
/// Event used when the grace period order is confirmed
/// </summary>
public record OrderStatusChangedToAwaitingValidationDomainEvent(
    Guid OrderId,
    OrderItem[] OrderItems) : INotification;
