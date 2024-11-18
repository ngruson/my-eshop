namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToCancelledIntegrationEvent(
    Guid OrderId, OrderStatus OrderStatus, string BuyerName, Guid BuyerIdentityGuid) : IntegrationEvent;
