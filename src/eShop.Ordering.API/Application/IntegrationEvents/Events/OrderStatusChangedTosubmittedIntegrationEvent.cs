namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToSubmittedIntegrationEvent(
    Guid OrderId, OrderStatus OrderStatus, string BuyerName, Guid BuyerIdentityGuid) : IntegrationEvent;
