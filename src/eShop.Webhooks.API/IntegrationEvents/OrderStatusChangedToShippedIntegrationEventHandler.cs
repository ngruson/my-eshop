namespace eShop.Webhooks.API.IntegrationEvents;

public class OrderStatusChangedToShippedIntegrationEventHandler(
    IWebhooksRetriever retriever,
    IWebhooksSender sender,
    ILogger<OrderStatusChangedToShippedIntegrationEventHandler> logger) : IIntegrationEventHandler<OrderStatusChangedToShippedIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToShippedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        IEnumerable<WebhookSubscription> subscriptions = await retriever.GetSubscriptionsOfType(WebhookType.OrderShipped);

        logger.LogInformation("Received OrderStatusChangedToShippedIntegrationEvent and got {SubscriptionCount} subscriptions to process", subscriptions.Count());

        WebhookData webhookData = new(WebhookType.OrderShipped, @event);

        await sender.SendAll(subscriptions, webhookData);
    }
}
