namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToAwaitingValidationIntegrationEvent(
    Guid OrderId, OrderStatus OrderStatus, string? BuyerName, Guid? BuyerIdentityGuid,
    OrderStockItem[] OrderStockItems, string? WorkflowInstanceId) : IntegrationEvent;

public record OrderStockItem(Guid ProductId, int Units);
