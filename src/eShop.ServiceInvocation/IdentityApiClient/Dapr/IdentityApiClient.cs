using Dapr.Client;
using eShop.ServiceInvocation.Auth;

namespace eShop.ServiceInvocation.IdentityApiClient.Dapr;

public class IdentityApiClient(DaprClient daprClient, AccessTokenAccessorFactory accessTokenAccessorFactory)
    : BaseDaprApiClient(daprClient, accessTokenAccessorFactory), IIdentityApiClient
{
    private readonly string basePath = "api/user/";
    protected override string AppId => "identity-api";

    public async Task<Identity.Contracts.GetUsers.UserDto[]> GetUsers()
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            this.basePath);

        return await this.DaprClient.InvokeMethodAsync<Identity.Contracts.GetUsers.UserDto[]>(request);
    }

    public async Task<Identity.Contracts.GetUser.UserDto> GetUser(string name)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            $"{this.basePath}/{name}");

        return await this.DaprClient.InvokeMethodAsync<Identity.Contracts.GetUser.UserDto>(request);
    }
}
