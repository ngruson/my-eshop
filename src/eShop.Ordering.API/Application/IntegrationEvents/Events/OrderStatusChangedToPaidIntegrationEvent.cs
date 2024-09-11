namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToPaidIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }
    public OrderStatus OrderStatus { get; }
    public string? BuyerName { get; }
    public string? BuyerIdentityGuid { get; }
    public IEnumerable<OrderStockItem> OrderStockItems { get; }

    public OrderStatusChangedToPaidIntegrationEvent(int orderId,
        OrderStatus orderStatus, string? buyerName, string? buyerIdentityGuid,
        IEnumerable<OrderStockItem> orderStockItems)
    {
        this.OrderId = orderId;
        this.OrderStockItems = orderStockItems;
        this.OrderStatus = orderStatus;
        this.BuyerName = buyerName;
        this.BuyerIdentityGuid = buyerIdentityGuid;
    }
}
