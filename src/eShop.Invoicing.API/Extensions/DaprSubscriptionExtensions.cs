using eShop.EventBus.Dapr;
using eShop.EventBus.Options;
using eShop.Invoicing.API.Application.IntegrationEvents.Events;
using eShop.Shared.Features;

namespace eShop.Invoicing.API.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, FeaturesConfiguration features, EventBusOptions eventBusOptions)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        api.MapSubscribe<OrderStatusChangedToStockConfirmedIntegrationEvent>("/orderStockConfirmed", features, eventBusOptions);

        return api;
    }
}
