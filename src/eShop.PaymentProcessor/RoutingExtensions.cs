using eShop.PaymentProcessor.Contracts;

namespace eShop.PaymentProcessor;

internal static class RoutingExtensions
{
    public static RouteGroupBuilder MapApiV1(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.MapGroup("api/workflow").HasApiVersion(1.0);

        api.MapPost("/", (IOptionsMonitor<PaymentOptions> options) =>
        {
            return Results.Ok(options.CurrentValue.PaymentSucceeded ? PaymentStatus.Succeeded : PaymentStatus.Failed);
        });

        return api;
    }
}
