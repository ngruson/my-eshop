using eShop.Basket.API.IntegrationEvents.Events;
using eShop.EventBus.Dapr;
using eShop.EventBus.Options;
using eShop.Shared.Features;

namespace eShop.Basket.API.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, FeaturesConfiguration features, EventBusOptions eventBusOptions)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        api.MapSubscribe<OrderStartedIntegrationEvent>("/orderStarted", features, eventBusOptions);

        return api;
    }
}
