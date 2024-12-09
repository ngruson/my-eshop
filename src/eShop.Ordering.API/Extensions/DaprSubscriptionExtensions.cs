using eShop.EventBus.Dapr;
using eShop.EventBus.Options;
using eShop.Shared.Features;

namespace eShop.Ordering.API.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, FeaturesConfiguration features, EventBusOptions eventBusOptions)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        api.MapSubscribe<GracePeriodConfirmedIntegrationEvent>("/gracePeriodConfirmed", features, eventBusOptions);
        api.MapSubscribe<OrderPaymentFailedIntegrationEvent>("/orderPaymentFailed", features, eventBusOptions);
        api.MapSubscribe<OrderPaymentSucceededIntegrationEvent>("/orderPaymentSucceeded", features, eventBusOptions);
        api.MapSubscribe<OrderStockConfirmedIntegrationEvent>("/orderStockConfirmed", features, eventBusOptions);
        api.MapSubscribe<OrderStockRejectedIntegrationEvent>("/orderStockRejected", features, eventBusOptions);

        return api;
    }
}
