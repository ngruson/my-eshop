using Ardalis.Result;
using Dapr.Client;
using eShop.EventBus.Abstractions;
using eShop.EventBus.Events;
using eShop.Shared.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eShop.EventBus.Dapr;

public class DaprEventBus(
    ILogger<DaprEventBus> logger, DaprClient daprClient, IOptions<FeaturesConfiguration> features) : IEventBus
{
    public async Task<Result> PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Publishing integration event: {EventName} with id {Guid}",
                integrationEvent.GetType().Name, integrationEvent.Id);

            await daprClient.PublishEventAsync(
                features.Value.PublishSubscribe.PubsubName,
                features.Value.PublishSubscribe.TopicName,
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
