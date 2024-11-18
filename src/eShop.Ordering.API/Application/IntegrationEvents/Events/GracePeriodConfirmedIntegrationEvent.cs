using System.Text.Json.Serialization;

namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record GracePeriodConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
