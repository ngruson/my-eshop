using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Dapr.Client;
using eShop.Identity.Contracts.GetUsers;
using eShop.Shared.Auth;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Dapr;

public class IdentityApiClientUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task return_users(
        [Substitute, Frozen] AccessTokenAccessor accessTokenAccessor,
        [Substitute, Frozen] DaprClient daprClient,
        IdentityApiClient.Dapr.IdentityApiClient sut,
        UserDto[] users,
        HttpRequestMessage httpRequestMessage,
        string accessToken
    )
    {
        // Arrange

        accessTokenAccessor.GetAccessTokenAsync()
                .Returns(accessToken);

        daprClient.CreateInvokeMethodRequest(
            HttpMethod.Get,
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
        .Returns(httpRequestMessage);

        daprClient.InvokeMethodAsync<UserDto[]>(httpRequestMessage)
            .Returns(users);

        // Act
        UserDto[] actual = await sut.GetUsers();

        // Assert
        Assert.Equal(actual, users);
    }
}
