namespace eShop.Ordering.Domain.Events;

public record OrderShippedDomainEvent(Order Order) : INotification;
