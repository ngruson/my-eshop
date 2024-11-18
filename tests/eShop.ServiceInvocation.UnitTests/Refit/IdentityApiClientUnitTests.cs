using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Identity.Contracts;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Refit;

public class IdentityApiClientUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task return_users(
        [Substitute, Frozen] IIdentityApi identityApi,
        IdentityApiClient.Refit.IdentityApiClient sut,
        Identity.Contracts.GetUsers.UserDto[] users
    )
    {
        // Arrange

        identityApi.GetUsers()
            .Returns(users);

        // Act

        Identity.Contracts.GetUsers.UserDto[] actual = await sut.GetUsers();

        // Assert

        Assert.Equal(actual, users);
    }

    [Theory, AutoNSubstituteData]
    public async Task return_user(
        [Substitute, Frozen] IIdentityApi identityApi,
        IdentityApiClient.Refit.IdentityApiClient sut,
        Identity.Contracts.GetUser.UserDto user
    )
    {
        // Arrange

        identityApi.GetUser(user.UserName)
            .Returns(user);

        // Act
        Identity.Contracts.GetUser.UserDto actual = await sut.GetUser(user.UserName);

        // Assert
        Assert.Equal(actual, user);
    }
}
