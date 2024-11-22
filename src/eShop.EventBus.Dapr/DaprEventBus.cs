using Ardalis.Result;
using Dapr.Client;
using eShop.EventBus.Abstractions;
using eShop.EventBus.Events;
using Microsoft.Extensions.Logging;

namespace eShop.EventBus.Dapr;

public class DaprEventBus(
    ILogger<DaprEventBus> logger,
    DaprClient daprClient) : IEventBus
{
    private readonly string topicName = "eShop_event_bus";

    public async Task<Result> PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Publishing integration event: {EventName} with id {Guid}",
                integrationEvent.GetType().Name, integrationEvent.Id);

            await daprClient.PublishEventAsync(
                "pubsub",
                this.topicName,
                Convert.ChangeType(integrationEvent, integrationEvent.GetType()),
                new Dictionary<string, string> {
                    { "routingKey", integrationEvent.GetType().Name }
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
