namespace eShop.ServiceInvocation.WebAppApiClient.Refit;

public class WebAppApiClient(IWebAppApi webAppApi) : IWebAppApiClient
{
    public async Task NotifyOrderStatusChange(string buyerIdentityGuid)
    {
        await webAppApi.NotifyOrderStatusChange(buyerIdentityGuid);
    }
}
