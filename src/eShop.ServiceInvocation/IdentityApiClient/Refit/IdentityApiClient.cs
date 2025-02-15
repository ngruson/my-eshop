using eShop.Identity.Contracts;
using eShop.Identity.Contracts.GetUsers;

namespace eShop.ServiceInvocation.IdentityApiClient.Refit;

public class IdentityApiClient(IIdentityApi identityApi) : IIdentityApiClient
{
    public async Task<UserDto[]> GetUsers()
    {
        return await identityApi.GetUsers();
    }

    public async Task<Identity.Contracts.GetUser.UserDto> GetUser(string name)
    {
        return await identityApi.GetUser(name);
    }
}
