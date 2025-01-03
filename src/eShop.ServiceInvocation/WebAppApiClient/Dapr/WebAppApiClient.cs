using Dapr.Client;
using eShop.ServiceInvocation.Auth;

namespace eShop.ServiceInvocation.WebAppApiClient.Dapr;

public class WebAppApiClient(DaprClient daprClient, AccessTokenAccessorFactory accessTokenAccessorFactory)
    : BaseDaprApiClient(daprClient, accessTokenAccessorFactory), IWebAppApiClient
{
    private readonly string basePath = "/api/orderStatus";
    protected override string AppId => "webapp";

    public async Task NotifyOrderStatusChange(string buyerIdentityGuid)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            $"{this.basePath}/{buyerIdentityGuid}");

        await this.DaprClient.InvokeMethodAsync(request);
    }
}
