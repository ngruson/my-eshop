using eShop.Identity.Contracts.GetUsers;

namespace eShop.ServiceInvocation.IdentityService;

public interface IIdentityService
{
    Task<UserDto[]> GetUsers();
}
