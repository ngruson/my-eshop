namespace eShop.Webhooks.API.IntegrationEvents;

public class OrderStatusChangedToPaidIntegrationEventHandler(
    IWebhooksRetriever retriever,
    IWebhooksSender sender,
    ILogger<OrderStatusChangedToShippedIntegrationEventHandler> logger) : IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToPaidIntegrationEvent @event, CancellationToken cancellationToken)
    {
        IEnumerable<WebhookSubscription> subscriptions = await retriever.GetSubscriptionsOfType(WebhookType.OrderPaid);

        logger.LogInformation("Received OrderStatusChangedToShippedIntegrationEvent and got {SubscriptionsCount} subscriptions to process", subscriptions.Count());

        WebhookData webhookData = new(WebhookType.OrderPaid, @event);

        await sender.SendAll(subscriptions, webhookData);
    }
}
