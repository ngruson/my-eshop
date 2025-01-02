namespace eShop.Catalog.API.IntegrationEvents.Events;

public record OrderStockRejectedIntegrationEvent(Guid OrderId, Contracts.AssessStockItemsForOrder.ConfirmedOrderStockItem[] OrderStockItems) : IntegrationEvent;
