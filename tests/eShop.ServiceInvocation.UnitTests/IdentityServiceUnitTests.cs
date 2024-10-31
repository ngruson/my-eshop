using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Identity.Contracts;
using eShop.Identity.Contracts.GetUsers;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests;

public class IdentityServiceUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task return_users(
        [Substitute, Frozen] IIdentityApi identityApi,
        IdentityApiClient.Refit.IdentityApiClient sut,
        UserDto[] users
    )
    {
        // Arrange

        identityApi.GetUsers()
            .Returns(users);

        // Act
        UserDto[] actual = await sut.GetUsers();

        // Assert
        Assert.Equal(actual, users);
    }
}
