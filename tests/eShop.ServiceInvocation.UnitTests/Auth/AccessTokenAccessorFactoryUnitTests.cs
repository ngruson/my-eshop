using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Duende.AccessTokenManagement;
using eShop.ServiceInvocation.Auth;
using eShop.ServiceInvocation.Options;
using eShop.Shared.DI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using static IdentityModel.OidcConstants;

namespace eShop.ServiceInvocation.UnitTests.Auth;

public class AccessTokenAccessorFactoryUnitTests
{
    [Theory, AutoNSubstituteData]
    public void client_credentials_accessor(        
        [Substitute, Frozen] ServiceProviderWrapper serviceProvider,
        [Substitute, Frozen] ServiceInvocationOptions serviceInvocationOptions,
        [Substitute, Frozen] IOptions<ServiceInvocationOptions> options,
        AccessTokenAccessorFactory sut,
        IClientCredentialsTokenManagementService tokenService)
    {
        // Arrange

        serviceInvocationOptions.GrantType = GrantTypes.ClientCredentials;
        options.Value.Returns(serviceInvocationOptions);

        serviceProvider.GetRequiredService<IClientCredentialsTokenManagementService>().Returns(tokenService);        

        // Act

        IAccessTokenAccessor accessTokenAccessor = sut.Create();

        // Assert

        Assert.IsType<ClientCredentialsAccessTokenAccessor>(accessTokenAccessor);
    }

    [Theory, AutoNSubstituteData]
    public void http_context_accessor(
        [Substitute, Frozen] ServiceProviderWrapper serviceProvider,
        [Substitute, Frozen] ServiceInvocationOptions serviceInvocationOptions,
        [Substitute, Frozen] IOptions<ServiceInvocationOptions> options,
        AccessTokenAccessorFactory sut,
        IHttpContextAccessor httpContextAccessor)
    {
        // Arrange

        serviceProvider.GetRequiredService<IHttpContextAccessor>().Returns(httpContextAccessor);
        options.Value.Returns(serviceInvocationOptions);

        // Act

        IAccessTokenAccessor accessTokenAccessor = sut.Create();

        // Assert

        Assert.IsType<HttpContextAccessTokenAccessor>(accessTokenAccessor);
    }
}
