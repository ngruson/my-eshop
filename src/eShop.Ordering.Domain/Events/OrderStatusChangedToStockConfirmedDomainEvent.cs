namespace eShop.Ordering.Domain.Events;

/// <summary>
/// Event used when the order stock items are confirmed
/// </summary>
public record OrderStatusChangedToStockConfirmedDomainEvent(Guid OrderId) : INotification;
