using eShop.EventBus.Dapr;
using eShop.EventBus.Options;
using eShop.Shared.Features;

namespace eShop.Webhooks.API.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, FeaturesConfiguration features, EventBusOptions eventBusOptions)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        api.MapSubscribe<OrderStatusChangedToPaidIntegrationEvent>("/orderStatusChangedToPaid", features, eventBusOptions);
        api.MapSubscribe<OrderStatusChangedToShippedIntegrationEvent>("/orderStatusChangedToShipped", features, eventBusOptions);
        api.MapSubscribe<ProductPriceChangedIntegrationEvent>("/productPriceChanged", features, eventBusOptions);

        return api;
    }
}
