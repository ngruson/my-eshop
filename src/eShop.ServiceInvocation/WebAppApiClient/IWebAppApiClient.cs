namespace eShop.ServiceInvocation.WebAppApiClient;

public interface IWebAppApiClient
{
    Task NotifyOrderStatusChange(string buyerIdentityGuid);
}
