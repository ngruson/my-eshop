using eShop.Identity.Contracts.CreateUser;
using eShop.Identity.Contracts.GetUsers;
using Refit;

namespace eShop.Identity.Contracts;

public interface IIdentityApi
{
    [Get("/api/user?api-version=1.0")]
    Task<UserDto[]> GetUsers();

    [Post("/api/user?api-version=1.0")]
    Task CreateUser([Body] CreateUserDto dto);
}
