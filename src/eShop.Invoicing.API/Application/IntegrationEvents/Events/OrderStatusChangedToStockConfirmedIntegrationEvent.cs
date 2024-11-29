using eShop.EventBus.Events;

namespace eShop.Invoicing.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToStockConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
