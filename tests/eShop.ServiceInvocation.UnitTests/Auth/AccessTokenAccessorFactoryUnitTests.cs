using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Duende.AccessTokenManagement;
using eShop.ServiceInvocation.Auth;
using eShop.ServiceInvocation.Options;
using eShop.Shared.DI;
using Microsoft.Extensions.Options;
using NSubstitute;
using static IdentityModel.OidcConstants;

namespace eShop.ServiceInvocation.UnitTests.Auth;

public class AccessTokenAccessorFactoryUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task return_catalogItem(        
        [Substitute, Frozen] ServiceProviderWrapper serviceProvider,
        [Substitute, Frozen] ServiceInvocationOptions serviceInvocationOptions,
        [Substitute, Frozen] IOptions<ServiceInvocationOptions> options,
        AccessTokenAccessorFactory sut,
        IClientCredentialsTokenManagementService tokenService)
    {
        // Arrange

        serviceInvocationOptions.GrantType = GrantTypes.ClientCredentials;
        options.Value.Returns(serviceInvocationOptions);

        //serviceProvider.GetRequiredService<IClientCredentialsTokenManagementService>()
        //.Returns(tokenService);        

        // Act

        IAccessTokenAccessor accessTokenAccessor = sut.Create();

        // Assert

        Assert.IsType<ClientCredentialsAccessTokenAccessor>(accessTokenAccessor);
    }
}
