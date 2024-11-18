namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStockConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
