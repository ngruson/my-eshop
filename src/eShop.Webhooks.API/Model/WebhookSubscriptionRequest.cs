namespace eShop.Webhooks.API.Model;

public class WebhookSubscriptionRequest : IValidatableObject
{
    public string? Url { get; set; }
    public string? Token { get; set; }
    public string? Event { get; set; }
    public string? GrantUrl { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Uri.IsWellFormedUriString(this.GrantUrl, UriKind.Absolute))
        {
            yield return new ValidationResult("GrantUrl is not valid", [nameof(this.GrantUrl)]);
        }

        if (!Uri.IsWellFormedUriString(this.Url, UriKind.Absolute))
        {
            yield return new ValidationResult("Url is not valid", [nameof(this.Url)]);
        }

        if (!Enum.TryParse(this.Event, ignoreCase: true, out WebhookType _))
        {
            yield return new ValidationResult($"{this.Event} is invalid event name", [nameof(this.Event)]);
        }
    }
}
