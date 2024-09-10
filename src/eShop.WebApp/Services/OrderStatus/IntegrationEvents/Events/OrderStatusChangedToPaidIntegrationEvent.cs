using eShop.EventBus.Events;

namespace eShop.WebApp.Services.OrderStatus.IntegrationEvents;

public record OrderStatusChangedToPaidIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }
    public string OrderStatus { get; }
    public string? BuyerName { get; }
    public string? BuyerIdentityGuid { get; }

    public OrderStatusChangedToPaidIntegrationEvent(
        int orderId, string orderStatus, string buyerName, string buyerIdentityGuid)
    {
        this.OrderId = orderId;
        this.OrderStatus = orderStatus;
        this.BuyerName = buyerName;
        this.BuyerIdentityGuid = buyerIdentityGuid;
    }
}
