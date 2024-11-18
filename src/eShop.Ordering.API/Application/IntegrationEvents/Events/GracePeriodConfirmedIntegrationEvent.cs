using System.Text.Json.Serialization;

namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record GracePeriodConfirmedIntegrationEvent : IntegrationEvent
{
    [JsonInclude]
    public int OrderId { get; }

    public GracePeriodConfirmedIntegrationEvent(int orderId) =>
        OrderId = orderId;
}

