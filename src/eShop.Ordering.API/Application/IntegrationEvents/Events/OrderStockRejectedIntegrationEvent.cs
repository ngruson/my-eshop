namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStockRejectedIntegrationEvent(int OrderId, List<ConfirmedOrderStockItem> OrderStockItems) : IntegrationEvent;
public record ConfirmedOrderStockItem(Guid ProductId, bool HasStock);
