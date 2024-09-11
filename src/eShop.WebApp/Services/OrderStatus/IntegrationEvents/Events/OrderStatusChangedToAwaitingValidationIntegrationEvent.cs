using eShop.EventBus.Events;

namespace eShop.WebApp.Services.OrderStatus.IntegrationEvents.Events;

public record OrderStatusChangedToAwaitingValidationIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }
    public string OrderStatus { get; }
    public string? BuyerName { get; }
    public string? BuyerIdentityGuid { get; }

    public OrderStatusChangedToAwaitingValidationIntegrationEvent(
        int orderId, string orderStatus, string? buyerName, string? buyerIdentityGuid)
    {
        this.OrderId = orderId;
        this.OrderStatus = orderStatus;
        this.BuyerName = buyerName;
        this.BuyerIdentityGuid = buyerIdentityGuid;
    }
}
