using eShop.Webhooks.API.Model;
using eShop.Webhooks.API.Services;

namespace eShop.Webhooks.API.IntegrationEvents;

public class OrderStatusChangedToShippedIntegrationEventHandler(
    IWebhooksRetriever retriever,
    IWebhooksSender sender,
    ILogger<OrderStatusChangedToShippedIntegrationEventHandler> logger) : IIntegrationEventHandler<OrderStatusChangedToShippedIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToShippedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var subscriptions = await retriever.GetSubscriptionsOfType(WebhookType.OrderShipped);

        logger.LogInformation("Received OrderStatusChangedToShippedIntegrationEvent and got {SubscriptionCount} subscriptions to process", subscriptions.Count());

        var webhookData = new WebhookData(WebhookType.OrderShipped, @event);

        await sender.SendAll(subscriptions, webhookData);
    }
}
