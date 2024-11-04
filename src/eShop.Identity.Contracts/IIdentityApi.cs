using eShop.Identity.Contracts.CreateUser;
using Refit;

namespace eShop.Identity.Contracts;

public interface IIdentityApi
{
    [Get("/api/user?api-version=1.0")]
    Task<GetUsers.UserDto[]> GetUsers();

    [Get("/api/user/{name}?api-version=1.0")]
    Task<GetUser.UserDto> GetUser(string name);

    [Post("/api/user?api-version=1.0")]
    Task CreateUser([Body] CreateUserDto dto);
}
