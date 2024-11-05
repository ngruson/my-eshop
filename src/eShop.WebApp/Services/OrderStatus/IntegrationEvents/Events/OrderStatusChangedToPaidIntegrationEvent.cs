using eShop.EventBus.Events;

namespace eShop.WebApp.Services.OrderStatus.IntegrationEvents.Events;

public record OrderStatusChangedToPaidIntegrationEvent(
    Guid OrderId, string OrderStatus, string BuyerName, Guid BuyerIdentityGuid) : IntegrationEvent;
