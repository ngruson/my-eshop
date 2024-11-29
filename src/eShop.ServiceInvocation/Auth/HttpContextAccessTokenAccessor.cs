using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.ServiceInvocation.Auth;

internal class HttpContextAccessTokenAccessor(IHttpContextAccessor httpContextAccessor) : IAccessTokenAccessor
{
    public async Task<string?> GetAccessToken()
    {
        if (httpContextAccessor.HttpContext is HttpContext httpContext)
        {
            IAuthenticationService? authenticationService = httpContext.RequestServices.GetService<IAuthenticationService>();
            if (authenticationService != null)
            {
                AuthenticateResult result = await authenticationService.AuthenticateAsync(httpContext, null);
                string? accessToken = result.Properties?.GetTokenValue("access_token");
                return accessToken ?? string.Empty;
            }
        }

        return null;
    }
}
