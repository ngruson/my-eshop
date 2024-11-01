using eShop.Identity.Contracts.GetUsers;

namespace eShop.ServiceInvocation.IdentityApiClient;

public interface IIdentityApiClient
{
    Task<UserDto[]> GetUsers();
}
