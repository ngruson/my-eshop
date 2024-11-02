using Ardalis.Result.AspNetCore;
using eShop.Identity.API.Api.Commands.CreateUser;
using eShop.Identity.API.Api.Queries.GetUser;
using eShop.Identity.API.Api.Queries.GetUsers;
using eShop.Identity.Contracts.CreateUser;
using MediatR;

namespace eShop.Identity.API;

internal static class IdentityApi
{
    public static RouteGroupBuilder MapIdentityApiV1(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.MapGroup("api/user").HasApiVersion(1.0);

        api.MapGet("/", async ([FromServices] IMediator mediator) =>
            (await mediator.Send(new GetUsersQuery()))
                .ToMinimalApiResult());

        api.MapGet("/{userName}", async (string userName, [FromServices] IMediator mediator) =>
            (await mediator.Send(new GetUserQuery(userName)))
                .ToMinimalApiResult());

        api.MapPost("/", async ([FromBody] CreateUserDto dto, [FromServices] IMediator mediator) =>
            (await mediator.Send(new CreateUserCommand(dto)))
                .ToMinimalApiResult());

        return api;
    }
}
