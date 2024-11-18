using System.Text.Json;
using Dapr;
using eShop.EventBus.Options;
using Microsoft.Extensions.Options;

namespace eShop.Ordering.API.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, IOptions<EventBusOptions> options)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        string[] routingKeys = [
            nameof(GracePeriodConfirmedIntegrationEvent),
            nameof(OrderPaymentFailedIntegrationEvent),
            nameof(OrderPaymentSucceededIntegrationEvent),
            nameof(OrderStockConfirmedIntegrationEvent),
            nameof(OrderStockRejectedIntegrationEvent)];

        api.MapPost("/handleIntegrationEvent", async (JsonDocument doc, [FromServices] IServiceProvider serviceProvider) =>
        {
            (IntegrationEvent? integrationEvent, IIntegrationEventHandler? eventHandler) =
                ParseMessage(doc, serviceProvider);

            if (integrationEvent is not null && eventHandler is not null)
            {
                await eventHandler.Handle(integrationEvent, default);
            }
        })
        .WithTopic(new TopicOptions
        {
            Metadata = new Dictionary<string, string>()
            {
                { "queueName", options.Value.SubscriptionClientName },
                { "routingKey", string.Join(',', routingKeys) }
            },
            Name = "eShop_event_bus",
            PubsubName = "pubsub"
        });

        return api;
    }

    private static (IntegrationEvent?, IIntegrationEventHandler?) ParseMessage(JsonDocument doc, IServiceProvider serviceProvider)
    {
        if (doc.RootElement.TryGetProperty("MessageType", out JsonElement messageType))
        {
            IntegrationEvent? integrationEvent;

            switch (messageType.GetString())
            {
                case nameof(GracePeriodConfirmedIntegrationEvent):
                    integrationEvent = JsonSerializer.Deserialize<GracePeriodConfirmedIntegrationEvent>(doc);
                    break;

                case nameof(OrderPaymentFailedIntegrationEvent):
                    integrationEvent = JsonSerializer.Deserialize<OrderPaymentFailedIntegrationEvent>(doc);
                    break;

                case nameof(OrderPaymentSucceededIntegrationEvent):
                    integrationEvent = JsonSerializer.Deserialize<OrderPaymentSucceededIntegrationEvent>(doc);
                    break;

                case nameof(OrderStockConfirmedIntegrationEvent):
                    integrationEvent = JsonSerializer.Deserialize<OrderStockConfirmedIntegrationEvent>(doc);
                    break;

                case nameof(OrderStockRejectedIntegrationEvent):
                    integrationEvent = JsonSerializer.Deserialize<OrderStockRejectedIntegrationEvent>(doc);
                    break;

                default: return (null, null);
            }

            IIntegrationEventHandler eventHandler =
                serviceProvider.GetRequiredKeyedService<IIntegrationEventHandler>(integrationEvent?.GetType());

            return (integrationEvent, eventHandler);
        }

        return (null, null);
    }
}
