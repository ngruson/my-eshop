namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStockRejectedIntegrationEvent(Guid OrderId, List<ConfirmedOrderStockItem> OrderStockItems) : IntegrationEvent;
public record ConfirmedOrderStockItem(Guid ProductId, bool HasStock);
