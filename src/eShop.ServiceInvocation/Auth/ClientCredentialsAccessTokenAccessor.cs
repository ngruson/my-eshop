using Duende.AccessTokenManagement;
using eShop.ServiceInvocation.Options;
using Microsoft.Extensions.Options;

namespace eShop.ServiceInvocation.Auth;

internal class ClientCredentialsAccessTokenAccessor(
    IClientCredentialsTokenManagementService tokenManagementService,
    IOptions<ServiceInvocationOptions> options) : IAccessTokenAccessor
{
    public async Task<string?> GetAccessToken()
    {
        ClientCredentialsToken token = await tokenManagementService.GetAccessTokenAsync(options.Value.ClientName);
        return token.AccessToken;
    }
}
