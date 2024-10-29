using eShop.Identity.Contracts;
using eShop.Identity.Contracts.GetUsers;

namespace eShop.ServiceInvocation.IdentityService.Refit;

public class IdentityService(IIdentityApi identityApi) : IIdentityService
{
    public async Task<UserDto[]> GetUsers()
    {
        return await identityApi.GetUsers();
    }
}
