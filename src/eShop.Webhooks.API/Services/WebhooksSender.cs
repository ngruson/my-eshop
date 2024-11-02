namespace eShop.Webhooks.API.Services;

public class WebhooksSender(IHttpClientFactory httpClientFactory, ILogger<WebhooksSender> logger) : IWebhooksSender
{
    public async Task SendAll(IEnumerable<WebhookSubscription> receivers, WebhookData data)
    {
        HttpClient client = httpClientFactory.CreateClient();
        string json = JsonSerializer.Serialize(data);
        IEnumerable<Task> tasks = receivers.Select(r => this.OnSendData(r, json, client));
        await Task.WhenAll(tasks);
    }

    private Task<HttpResponseMessage> OnSendData(WebhookSubscription subs, string jsonData, HttpClient client)
    {
        HttpRequestMessage request = new()
        {
            RequestUri = new Uri(subs.DestUrl!, UriKind.Absolute),
            Method = HttpMethod.Post,
            Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
        };

        if (!string.IsNullOrWhiteSpace(subs.Token))
        {
            request.Headers.Add("X-eshop-whtoken", subs.Token);
        }

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Sending hook to {DestUrl} of type {Type}", subs.DestUrl, subs.Type);
        }

        return client.SendAsync(request);
    }

}
