namespace eShop.ServiceInvocation.Auth;

public interface IAccessTokenAccessor
{
    Task<string?> GetAccessToken();
}
