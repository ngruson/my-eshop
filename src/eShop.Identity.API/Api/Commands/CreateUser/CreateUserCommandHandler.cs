using Ardalis.Result;
using MediatR;

namespace eShop.Identity.API.Api.Commands.CreateUser;

public class CreateUserCommandHandler(
    ILogger<CreateUserCommandHandler> logger, UserManager<ApplicationUser> userManager)
        : IRequestHandler<CreateUserCommand, Result>
{
    private readonly ILogger<CreateUserCommandHandler> logger = logger;
    private readonly UserManager<ApplicationUser> userManager = userManager;

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ApplicationUser? user = await this.userManager.FindByNameAsync(request.Dto.UserName);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = request.Dto.UserName,
                    Email = request.Dto.Email,
                    EmailConfirmed = true,
                    Id = Guid.NewGuid().ToString(),
                    PhoneNumber = request.Dto.PhoneNumber,
                    FirstName = request.Dto.FirstName,
                    LastName = request.Dto.LastName
                };

                IdentityResult identityResult = await this.userManager.CreateAsync(user, "Pass123$");

                if (!identityResult.Succeeded)
                {
                    return Result.Error("Failed to create user");
                }

                return Result.Success();
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to create user");
            return Result.Error(ex.Message);
        }
    }
}
