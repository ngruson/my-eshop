namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToStockConfirmedIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }
    public OrderStatus OrderStatus { get; }
    public string? BuyerName { get; }
    public string? BuyerIdentityGuid { get; }

    public OrderStatusChangedToStockConfirmedIntegrationEvent(
        int orderId, OrderStatus orderStatus, string? buyerName, string? buyerIdentityGuid)
    {
        this.OrderId = orderId;
        this.OrderStatus = orderStatus;
        this.BuyerName = buyerName;
        this.BuyerIdentityGuid = buyerIdentityGuid;
    }
}
