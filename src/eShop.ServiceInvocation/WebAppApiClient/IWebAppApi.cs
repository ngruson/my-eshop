using Refit;

namespace eShop.ServiceInvocation.WebAppApiClient;

public interface IWebAppApi
{
    [Post("/api/orderStatus/{buyerIdentityGuid}?api-version=1.0")]
    Task NotifyOrderStatusChange(string buyerIdentityGuid);
}
