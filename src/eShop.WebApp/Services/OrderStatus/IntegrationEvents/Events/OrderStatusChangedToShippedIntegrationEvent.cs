using eShop.EventBus.Events;

namespace eShop.WebApp.Services.OrderStatus.IntegrationEvents;

public record OrderStatusChangedToShippedIntegrationEvent(
    Guid OrderId, string OrderStatus, string BuyerName, Guid BuyerIdentityGuid) : IntegrationEvent;
