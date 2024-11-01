namespace eShop.Identity.API.Models.ConsentViewModels;

public class ProcessConsentResult
{
    public bool IsRedirect => RedirectUri != null;
    public string? RedirectUri { get; set; }
    public Client? Client { get; set; }

    public bool ShowView => this.ViewModel != null;
    public ConsentViewModel? ViewModel { get; set; }

    public bool HasValidationError => this.ValidationError != null;
    public string? ValidationError { get; set; }
}
