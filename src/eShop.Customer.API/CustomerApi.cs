using Ardalis.Result.AspNetCore;
using eShop.Customer.API.Application.Queries.GetCustomers;

namespace eShop.Customer.API;

internal static class CustomerApi
{
    public static RouteGroupBuilder MapCustomerApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/customers").HasApiVersion(1.0);

        api.MapGet("/all", async ([FromServices] IMediator mediator) =>
            (await mediator.Send(new GetCustomersQuery()))
                .ToMinimalApiResult());

        return api;
    }
}
