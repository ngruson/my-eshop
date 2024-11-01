namespace eShop.Webhooks.API.Model;

public class WebhookData(WebhookType hookType, object data)
{
    public DateTime When { get; } = DateTime.UtcNow;

    public string Payload { get; } = JsonSerializer.Serialize(data);

    public string Type { get; } = hookType.ToString();
}
