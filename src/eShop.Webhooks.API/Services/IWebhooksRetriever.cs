namespace eShop.Webhooks.API.Services;

public interface IWebhooksRetriever
{

    Task<IEnumerable<WebhookSubscription>> GetSubscriptionsOfType(WebhookType type);
}
