namespace eShop.Webhooks.API.IntegrationEvents;

public record OrderStatusChangedToShippedIntegrationEvent(Guid OrderId, string OrderStatus, string BuyerName) : IntegrationEvent;
