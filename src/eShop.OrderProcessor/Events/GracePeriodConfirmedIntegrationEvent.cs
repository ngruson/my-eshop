namespace eShop.OrderProcessor.Events;

using eShop.EventBus.Events;

public record GracePeriodConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
