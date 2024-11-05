namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToAwaitingValidationIntegrationEvent(
    Guid OrderId, OrderStatus OrderStatus, string? BuyerName, Guid? BuyerIdentityGuid,
    OrderStockItem[] OrderStockItems) : IntegrationEvent;

public record OrderStockItem(Guid ProductId, int Units);
