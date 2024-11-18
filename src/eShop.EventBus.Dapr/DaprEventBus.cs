using System.Text;
using System.Text.Json;
using Ardalis.Result;
using Dapr.Client;
using eShop.EventBus.Abstractions;
using eShop.EventBus.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eShop.EventBus.Dapr;

public class DaprEventBus(
    ILogger<DaprEventBus> logger,
    DaprClient daprClient,    
    IOptions<EventBusSubscriptionInfo> subscriptionOptions) : IEventBus
{
    private readonly string topicName = "eShop_event_bus";

    public async Task<Result> PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Publishing integration event: {EventName} with id {Guid}", @event.GetType().Name, @event.Id);
            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), subscriptionOptions.Value.JsonSerializerOptions);
            string jsonString = Encoding.UTF8.GetString(jsonBytes);

            await daprClient.PublishEventAsync(
                "pubsub",
                this.topicName,
                jsonString,
                new Dictionary<string, string> {
                    { "routingKey", @event.GetType().Name }
                },
                cancellationToken);
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to publish integration event.";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
        
        return Result.Success();
    }
}
