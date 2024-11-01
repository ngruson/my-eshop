using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace eShop.Shared.Auth;

public class AccessTokenAccessor(IHttpContextAccessor httpContextAccessor)
{
    public virtual async Task<string?> GetAccessTokenAsync()
    {
        if (httpContextAccessor.HttpContext is HttpContext context)
        {
            string? accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            return accessToken ?? string.Empty;
        }

        return null;
    }
}
