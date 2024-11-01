using Dapr.Client;
using eShop.Identity.Contracts.GetUsers;
using eShop.Shared.Auth;

namespace eShop.ServiceInvocation.IdentityApiClient.Dapr;

public class IdentityApiClient(DaprClient daprClient, AccessTokenAccessor accessTokenAccessor)
    : BaseDaprApiClient(daprClient, accessTokenAccessor), IIdentityApiClient
{
    private readonly string basePath = "api/user/";
    protected override string AppId => "identity-api";

    public async Task<UserDto[]> GetUsers()
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            this.basePath);

        return await this.DaprClient.InvokeMethodAsync<UserDto[]>(request);
    }
}
