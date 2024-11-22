using eShop.EventBus.Dapr;
using eShop.EventBus.Options;

namespace eShop.PaymentProcessor.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, IOptions<EventBusOptions> options)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        api.MapSubscribe<OrderStatusChangedToStockConfirmedIntegrationEvent>(
            "/orderStatusChangedToStockConfirmed", options);

        return api;
    }
}
