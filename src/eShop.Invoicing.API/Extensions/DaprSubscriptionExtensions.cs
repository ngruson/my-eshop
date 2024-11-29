using eShop.EventBus.Dapr;
using eShop.EventBus.Options;
using eShop.Invoicing.API.Application.IntegrationEvents.Events;
using Microsoft.Extensions.Options;

namespace eShop.Invoicing.API.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, IOptions<EventBusOptions> options)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        api.MapSubscribe<OrderStatusChangedToStockConfirmedIntegrationEvent>("/orderStockConfirmed", options);

        return api;
    }
}
