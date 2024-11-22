using eShop.EventBus.Dapr;
using eShop.EventBus.Options;
using eShop.WebApp.Services.OrderStatus.IntegrationEvents.Events;
using Microsoft.Extensions.Options;

namespace eShop.WebApp.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, IOptions<EventBusOptions> options)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        api.MapSubscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent>("/orderStatusChangedToAwaitingValidation", options);
        api.MapSubscribe<OrderStatusChangedToPaidIntegrationEvent>("/orderStatusChangedToPaid", options);
        api.MapSubscribe<OrderStatusChangedToStockConfirmedIntegrationEvent>("/orderStatusChangedToStockConfirmed", options);
        api.MapSubscribe<OrderStatusChangedToShippedIntegrationEvent>("/orderStatusChangedToShipped", options);
        api.MapSubscribe<OrderStatusChangedToCancelledIntegrationEvent>("/orderStatusChangedToCancelled", options);
        api.MapSubscribe<OrderStatusChangedToSubmittedIntegrationEvent>("/orderStatusChangedToSubmitted", options);

        return api;
    }
}
