using eShop.Identity.Contracts.CreateUser;
using Refit;

namespace eShop.Identity.Contracts;

public interface IIdentityApi
{
    [Post("/api/user?api-version=1.0")]
    Task CreateUser([Authorize("Bearer")] string accessToken, [Body] CreateUserDto dto);
}
