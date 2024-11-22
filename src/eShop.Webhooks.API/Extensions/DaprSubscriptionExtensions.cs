using eShop.EventBus.Dapr;
using eShop.EventBus.Options;
using Microsoft.Extensions.Options;

namespace eShop.Webhooks.API.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, IOptions<EventBusOptions> options)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        api.MapSubscribe<OrderStatusChangedToPaidIntegrationEvent>("/orderStatusChangedToPaid", options);
        api.MapSubscribe<OrderStatusChangedToShippedIntegrationEvent>("/orderStatusChangedToShipped", options);
        api.MapSubscribe<ProductPriceChangedIntegrationEvent>("/productPriceChanged", options);

        return api;
    }
}
