namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToShippedIntegrationEvent(
    Guid OrderId, OrderStatus OrderStatus, string BuyerName, Guid BuyerIdentityGuid) : IntegrationEvent;
