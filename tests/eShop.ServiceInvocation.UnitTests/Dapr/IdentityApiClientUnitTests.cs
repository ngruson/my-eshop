using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Dapr.Client;
using eShop.ServiceInvocation.Auth;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Dapr;

public class IdentityApiClientUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task return_users(
        [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
        [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
        [Substitute, Frozen] DaprClient daprClient,
        IdentityApiClient.Dapr.IdentityApiClient sut,
        Identity.Contracts.GetUsers.UserDto[] users,
        HttpRequestMessage httpRequestMessage,
        string accessToken
    )
    {
        // Arrange

        accessTokenAccessor.GetAccessToken().Returns(accessToken);
        accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

        daprClient.CreateInvokeMethodRequest(
            HttpMethod.Get,
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
        .Returns(httpRequestMessage);

        daprClient.InvokeMethodAsync<Identity.Contracts.GetUsers.UserDto[]>(httpRequestMessage)
            .Returns(users);

        // Act
        Identity.Contracts.GetUsers.UserDto[] actual = await sut.GetUsers();

        // Assert
        Assert.Equal(actual, users);
    }

    [Theory, AutoNSubstituteData]
    public async Task return_user(
        [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
        [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
        [Substitute, Frozen] DaprClient daprClient,
        IdentityApiClient.Dapr.IdentityApiClient sut,
        Identity.Contracts.GetUser.UserDto user,
        HttpRequestMessage httpRequestMessage,
        string accessToken
    )
    {
        // Arrange

        accessTokenAccessor.GetAccessToken().Returns(accessToken);
        accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

        daprClient.CreateInvokeMethodRequest(
            HttpMethod.Get,
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
        .Returns(httpRequestMessage);

        daprClient.InvokeMethodAsync<Identity.Contracts.GetUser.UserDto>(httpRequestMessage)
            .Returns(user);

        // Act
        Identity.Contracts.GetUser.UserDto actual = await sut.GetUser(user.UserName);

        // Assert
        Assert.Equal(actual, user);
    }
}
