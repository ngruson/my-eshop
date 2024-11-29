using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Duende.AccessTokenManagement;
using eShop.ServiceInvocation.Auth;
using eShop.ServiceInvocation.Options;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Auth;

public class ClientCredentialsAccessTokenAccessorUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_accesstoken_(
        [Substitute, Frozen] IClientCredentialsTokenManagementService tokenService,
        [Substitute, Frozen] ServiceInvocationOptions serviceInvocationOptions,
        [Substitute, Frozen] IOptions<ServiceInvocationOptions> options,
        ClientCredentialsAccessTokenAccessor sut,
        ClientCredentialsToken clientCredentialsToken)
    {
        // Arrange

        options.Value.Returns(serviceInvocationOptions);
        tokenService.GetAccessTokenAsync(serviceInvocationOptions.ClientName).Returns(clientCredentialsToken);

        // Act

        string? accessToken = await sut.GetAccessToken();

        // Assert

        Assert.Equal(clientCredentialsToken.AccessToken, accessToken);
    }
}
