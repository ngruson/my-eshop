namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToStockConfirmedIntegrationEvent(
    Guid OrderId,
    OrderStatus OrderStatus,
    string? BuyerName,
    Guid? BuyerIdentityGuid) : IntegrationEvent;
