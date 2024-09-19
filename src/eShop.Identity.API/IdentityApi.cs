using Ardalis.Result.AspNetCore;
using eShop.Identity.API.Api.Commands.CreateUser;
using eShop.Identity.Contracts.CreateUser;
using MediatR;

namespace eShop.Identity.API;

internal static class IdentityApi
{
    public static RouteGroupBuilder MapIdentityApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/user").HasApiVersion(1.0);

        api.MapPost("/", async ([FromBody] CreateUserDto dto, [FromServices] IMediator mediator) =>
            (await mediator.Send(new CreateUserCommand(dto)))
                .ToMinimalApiResult())
            .RequireAuthorization(AuthorizationTokenPolicyNames.UserPolicy);

        return api;
    }
}
