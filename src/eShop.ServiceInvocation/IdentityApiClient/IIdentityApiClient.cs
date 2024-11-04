namespace eShop.ServiceInvocation.IdentityApiClient;

public interface IIdentityApiClient
{
    Task<Identity.Contracts.GetUsers.UserDto[]> GetUsers();
    Task<Identity.Contracts.GetUser.UserDto> GetUser(string name);
}
