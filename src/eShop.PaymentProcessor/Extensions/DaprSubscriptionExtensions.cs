using eShop.EventBus.Dapr;
using eShop.EventBus.Options;
using eShop.Shared.Features;

namespace eShop.PaymentProcessor.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, FeaturesConfiguration features, EventBusOptions eventBusOptions)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        api.MapSubscribe<OrderStatusChangedToStockConfirmedIntegrationEvent>(
            "/orderStatusChangedToStockConfirmed", features, eventBusOptions);

        return api;
    }
}
