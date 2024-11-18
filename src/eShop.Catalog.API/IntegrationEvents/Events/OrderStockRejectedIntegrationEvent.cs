namespace eShop.Catalog.API.IntegrationEvents.Events;

public record OrderStockRejectedIntegrationEvent(Guid OrderId, List<ConfirmedOrderStockItem> OrderStockItems) : IntegrationEvent;
