using AutoFixture.AutoNSubstitute;
using Duende.IdentityServer;
using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.ResponseHandling;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using eShop.Identity.API.Quickstart;
using Microsoft.Extensions.Logging;

namespace eShop.Identity.UnitTests;

public class CustomAuthorizeInteractionResponseGeneratorUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task ProcessInteraction(
        [Substitute] IdentityServerOptions options,
        [Substitute] IClock clock,
        [Substitute] ILogger<CustomAuthorizeInteractionResponseGenerator> logger,
        [Substitute] IConsentService consent,
        [Substitute] IProfileService profile)
    {
        // Arrange

        CustomAuthorizeInteractionResponseGenerator sut = new(
            options,
            clock,
            logger,
            consent,
            profile);

        ValidatedAuthorizeRequest request = new()
        {
            ClientId = "adminApp"
        };

        // Act

        InteractionResponse? result = await sut.ProcessInteractionAsync(request);

        // Assert

        Assert.Equal("/account/loginEmployee", result.RedirectUrl);
    }
}
