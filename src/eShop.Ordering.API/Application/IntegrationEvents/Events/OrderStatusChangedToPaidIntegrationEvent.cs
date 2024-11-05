namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToPaidIntegrationEvent(
    Guid OrderId, OrderStatus OrderStatus, string BuyerName, Guid BuyerIdentityGuid,
    OrderStockItem[] OrderStockItems) : IntegrationEvent;
