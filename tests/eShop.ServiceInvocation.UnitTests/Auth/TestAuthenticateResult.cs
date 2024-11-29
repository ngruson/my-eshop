using Microsoft.AspNetCore.Authentication;

namespace eShop.ServiceInvocation.UnitTests.Auth;

internal class TestAuthenticateResult : AuthenticateResult
{
    public void AddAccessToken(string accessToken)
    {
        IDictionary<string, string?> dict = new Dictionary<string, string?>() { { ".Token.access_token", accessToken } };
        this.Properties = new AuthenticationProperties(dict);
    }
}
