namespace eShop.Ordering.Domain.Events;

public record BuyerAndPaymentMethodVerifiedDomainEvent(Buyer Buyer, PaymentMethod Payment, Order Order) : INotification;
