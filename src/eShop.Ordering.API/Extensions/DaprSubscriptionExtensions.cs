using eShop.EventBus.Dapr;
using eShop.EventBus.Options;
using Microsoft.Extensions.Options;

namespace eShop.Ordering.API.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, IOptions<EventBusOptions> options)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        api.MapSubscribe<GracePeriodConfirmedIntegrationEvent>("/gracePeriodConfirmed", options);
        api.MapSubscribe<OrderPaymentFailedIntegrationEvent>("/orderPaymentFailed", options);
        api.MapSubscribe<OrderPaymentSucceededIntegrationEvent>("/orderPaymentSucceeded", options);
        api.MapSubscribe<OrderStockConfirmedIntegrationEvent>("/orderStockConfirmed", options);
        api.MapSubscribe<OrderStockRejectedIntegrationEvent>("/orderStockRejected", options);

        return api;
    }
}
