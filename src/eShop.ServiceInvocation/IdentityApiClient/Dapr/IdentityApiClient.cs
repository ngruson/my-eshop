using Dapr.Client;
using eShop.Identity.Contracts.GetUsers;
using Microsoft.AspNetCore.Http;

namespace eShop.ServiceInvocation.IdentityApiClient.Dapr;

public class IdentityApiClient(DaprClient daprClient, IHttpContextAccessor httpContextAccessor)
    : BaseDaprApiClient(daprClient, httpContextAccessor), IIdentityApiClient
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
