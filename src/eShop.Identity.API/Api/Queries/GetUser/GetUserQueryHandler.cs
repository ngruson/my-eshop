using Ardalis.Result;
using eShop.Identity.API.Api.Commands.CreateUser;
using eShop.Identity.Contracts.GetUser;
using MediatR;

namespace eShop.Identity.API.Api.Queries.GetUser;

internal class GetUserQueryHandler(
    ILogger<CreateUserCommandHandler> logger,
    UserManager<ApplicationUser> userManager) : IRequestHandler<GetUserQuery, Result<UserDto>>
{
    private readonly ILogger<CreateUserCommandHandler> logger = logger;
    private readonly UserManager<ApplicationUser> userManager = userManager;

    public async Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            ApplicationUser? user = await this.userManager.FindByNameAsync(request.UserName);

            if (user is null)
            {
                return Result.NotFound();
            }

            UserDto userDto = new(
                user.Id,
                user.UserName!,
                user.FirstName!,
                user.LastName!);

            return userDto;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to get user");
            return Result.Error(ex.Message);
        }
    }
}
