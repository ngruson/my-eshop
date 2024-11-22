using eShop.EventBus.Dapr;
using eShop.EventBus.Options;

namespace eShop.Catalog.API.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, IOptions<EventBusOptions> options)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        api.MapSubscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent>(
            "/orderStatusChangedToAwaitingValidation", options);

        api.MapSubscribe<OrderStatusChangedToPaidIntegrationEvent>(
            "/orderStatusChangedToPaid", options);

        return api;
    }
}
