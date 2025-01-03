namespace eShop.Catalog.API.IntegrationEvents.Events;

public record OrderStatusChangedToAwaitingValidationIntegrationEvent(
    Guid OrderId,
    IEnumerable<OrderStockItem> OrderStockItems,
    string? WorkflowInstanceId) : IntegrationEvent;
