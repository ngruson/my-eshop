using Ardalis.Result;
using eShop.Identity.Contracts.GetUsers;
using MediatR;

namespace eShop.Identity.API.Api.Queries.GetUsers;

public class GetUsersQueryHandler(
    ILogger<GetUsersQueryHandler> logger,
    UserManager<ApplicationUser> userManager) : IRequestHandler<GetUsersQuery, Result<List<UserDto>>>
{
    private readonly ILogger<GetUsersQueryHandler> logger = logger;
    private readonly UserManager<ApplicationUser> userManager = userManager;

    public Task<Result<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Getting users...");

            List<ApplicationUser> tmp = [.. this.userManager.Users];

            List<UserDto> users =
                [.. this.userManager.Users.Select(_ => new UserDto(
                    _.Id, _.UserName!, _.FirstName!, _.LastName!))];

            this.logger.LogInformation("Users retrieved");

            return Task.FromResult(Result.Success(users));
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to get users");
            return Task.FromResult(Result<List<UserDto>>.Error(ex.Message));
        }
    }
}
