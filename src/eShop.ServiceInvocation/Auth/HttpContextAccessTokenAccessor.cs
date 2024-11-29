using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace eShop.ServiceInvocation.Auth;

internal class HttpContextAccessTokenAccessor(IHttpContextAccessor httpContextAccessor) : IAccessTokenAccessor
{
    public async Task<string?> GetAccessToken()
    {
        if (httpContextAccessor.HttpContext is not null)
        {
            string? accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            return accessToken ?? string.Empty;
        }

        return null;
    }
}
