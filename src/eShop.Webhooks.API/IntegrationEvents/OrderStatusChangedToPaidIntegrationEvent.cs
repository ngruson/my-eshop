namespace eShop.Webhooks.API.IntegrationEvents;

public record OrderStatusChangedToPaidIntegrationEvent(Guid OrderId, OrderStockItem[] OrderStockItems) : IntegrationEvent;
