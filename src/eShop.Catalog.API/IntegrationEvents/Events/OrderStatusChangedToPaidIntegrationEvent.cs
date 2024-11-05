namespace eShop.Catalog.API.IntegrationEvents.Events;

public record OrderStatusChangedToPaidIntegrationEvent(Guid OrderId, IEnumerable<OrderStockItem> OrderStockItems) : IntegrationEvent;
