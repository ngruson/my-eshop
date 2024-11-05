namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderPaymentFailedIntegrationEvent(Guid OrderId) : IntegrationEvent;
