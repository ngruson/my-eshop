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
    public async Task<Result> PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Publishing integration event: {EventName} with id {Guid}", @event.GetType().Name, @event.Id);
            string topicName = @event.GetType().Name;
            await daprClient.PublishEventAsync("pubSub", topicName, @event, cancellationToken);
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
