using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.ServiceInvocation.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Auth;

public class HttpContextAccessTokenAccessorUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_accesstoken_given_valid_accesstoken(
        [Substitute, Frozen] IAuthenticationService authenticationService,
        [Substitute, Frozen] IHttpContextAccessor httpContextAccessor,
        [Substitute, Frozen] HttpContext httpContext,
        HttpContextAccessTokenAccessor sut,
        TestAuthenticateResult authenticateResult,
        string token)
    {
        // Arrange

        authenticateResult.AddAccessToken(token);

        authenticationService.AuthenticateAsync(httpContext, null)
            .Returns(authenticateResult);

        httpContext.RequestServices.GetService(typeof(IAuthenticationService))
            .Returns(authenticationService);

        httpContextAccessor.HttpContext.Returns(httpContext);

        // Act

        string? accessToken = await sut.GetAccessToken();

        // Assert

        Assert.Equal(token, accessToken);
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_empty_string_when_no_accesstoken(
        [Substitute, Frozen] IAuthenticationService authenticationService,
        [Substitute, Frozen] IHttpContextAccessor httpContextAccessor,
        [Substitute, Frozen] HttpContext httpContext,
        HttpContextAccessTokenAccessor sut,
        AuthenticateResult authenticateResult)
    {
        // Arrange

        authenticationService.AuthenticateAsync(httpContext, null)
            .Returns(authenticateResult);

        httpContext.RequestServices.GetService(typeof(IAuthenticationService))
            .Returns(authenticationService);

        httpContextAccessor.HttpContext.Returns(httpContext);

        // Act

        string? accessToken = await sut.GetAccessToken();

        // Assert

        Assert.Empty(accessToken!);
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_null_when_no_authentication_service(
        HttpContextAccessTokenAccessor sut)
    {
        // Arrange

        // Act

        string? accessToken = await sut.GetAccessToken();

        // Assert

        Assert.Null(accessToken!);
    }
}
