namespace eShop.Ordering.Domain.Events;

public record OrderCancelledDomainEvent(Order Order) : INotification;
