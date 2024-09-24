using Duende.IdentityServer.ResponseHandling;

namespace eShop.Identity.API.Quickstart;

public class CustomAuthorizeInteractionResponseGenerator(
    IdentityServerOptions options,
    IClock clock,
    ILogger<AuthorizeInteractionResponseGenerator> logger,
    IConsentService consent,
    IProfileService profile) : AuthorizeInteractionResponseGenerator(options, clock, logger, consent, profile)
{
    protected override async Task<InteractionResponse> ProcessLoginAsync(ValidatedAuthorizeRequest request)
    {
        InteractionResponse result = await base.ProcessLoginAsync(request);

        if (!result.IsError && request.ClientId == "adminapp" && !request.Subject.IsAuthenticated())
        {
            result = new InteractionResponse { RedirectUrl = "/account/loginEmployee" };
        }

        return result;
    }
}
