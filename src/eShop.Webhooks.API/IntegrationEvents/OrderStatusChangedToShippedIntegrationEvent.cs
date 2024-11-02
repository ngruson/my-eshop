namespace eShop.Webhooks.API.IntegrationEvents;

public record OrderStatusChangedToShippedIntegrationEvent(int OrderId, string OrderStatus, string BuyerName) : IntegrationEvent;
