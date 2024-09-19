using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace eShop.Customer.FunctionalTests;

class AutoAuthorizeMiddleware(RequestDelegate rd)
{
    public const string IDENTITY_ID = "9e3163b9-1ae6-4652-9dc6-7898ab7b7a00";

    private readonly RequestDelegate _next = rd;

    public async Task Invoke(HttpContext httpContext)
    {
        ClaimsIdentity identity = new("cookies");

        identity.AddClaim(new Claim("sub", IDENTITY_ID));
        identity.AddClaim(new Claim("unique_name", IDENTITY_ID));
        identity.AddClaim(new Claim(ClaimTypes.Name, IDENTITY_ID));

        httpContext.User.AddIdentity(identity);

        await this._next.Invoke(httpContext);
    }
}
