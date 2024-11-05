namespace eShop.Ordering.Domain.Events;

/// <summary>
/// Event used when the order is paid
/// </summary>
public record OrderStatusChangedToPaidDomainEvent(Guid OrderId, OrderItem[] OrderItems) : INotification;
