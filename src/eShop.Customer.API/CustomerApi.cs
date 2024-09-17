using Ardalis.Result.AspNetCore;
using eShop.Customer.API.Application.Commands.CreateCustomer;
using eShop.Customer.API.Application.Commands.DeleteCustomer;
using eShop.Customer.API.Application.Commands.UpdateCustomer;
using eShop.Customer.API.Application.Queries.GetCustomer;
using eShop.Customer.API.Application.Queries.GetCustomers;
using eShop.Customer.Contracts.CreateCustomer;
using eShop.Customer.Contracts.UpdateCustomer;

namespace eShop.Customer.API;

internal static class CustomerApi
{
    public static RouteGroupBuilder MapCustomerApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/customers").HasApiVersion(1.0);

        api.MapGet("/all", async ([FromServices] IMediator mediator) =>
            (await mediator.Send(new GetCustomersQuery()))
                .ToMinimalApiResult());

        api.MapGet("/",
            async (string firstName, string lastName, [FromServices] IMediator mediator) =>
            (await mediator.Send(new GetCustomerQuery(firstName, lastName)))
                .ToMinimalApiResult());

        api.MapPost("/", async ([FromBody] CreateCustomerDto dto, [FromServices] IMediator mediator) =>
            (await mediator.Send(new CreateCustomerCommand(dto)))
                .ToMinimalApiResult());

        api.MapPut("/", async ([FromBody] UpdateCustomerDto dto, [FromServices] IMediator mediator) =>
            (await mediator.Send(new UpdateCustomerCommand(dto)))
                .ToMinimalApiResult());

        api.MapDelete("/", async ([FromQuery] string firstName, [FromQuery] string lastName, [FromServices] IMediator mediator) =>
            (await mediator.Send(new DeleteCustomerCommand(firstName, lastName)))
                .ToMinimalApiResult());

        return api;
    }
}
