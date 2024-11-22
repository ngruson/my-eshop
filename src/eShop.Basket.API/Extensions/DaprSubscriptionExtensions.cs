using eShop.Basket.API.IntegrationEvents.Events;
using eShop.EventBus.Dapr;
using eShop.EventBus.Options;
using Microsoft.Extensions.Options;

namespace eShop.Basket.API.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, IOptions<EventBusOptions> options)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        api.MapSubscribe<OrderStartedIntegrationEvent>("/orderStarted", options);

        return api;
    }
}
