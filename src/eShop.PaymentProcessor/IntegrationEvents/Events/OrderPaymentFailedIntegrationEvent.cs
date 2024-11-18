namespace eShop.PaymentProcessor.IntegrationEvents.Events;

public record OrderPaymentFailedIntegrationEvent(Guid OrderId) : IntegrationEvent;
