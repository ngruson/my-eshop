using eShop.EventBus.Dapr;
using eShop.EventBus.Options;
using eShop.Shared.Features;
using eShop.WebApp.Services.OrderStatus.IntegrationEvents.Events;

namespace eShop.WebApp.Extensions;

internal static class DaprSubscriptionExtensions
{
    public static RouteGroupBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder app, FeaturesConfiguration features, EventBusOptions eventBusOptions)
    {
        RouteGroupBuilder api = app.MapGroup("api/dapr");

        api.MapSubscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent>("/orderStatusChangedToAwaitingValidation", features, eventBusOptions);
        api.MapSubscribe<OrderStatusChangedToPaidIntegrationEvent>("/orderStatusChangedToPaid", features, eventBusOptions);
        api.MapSubscribe<OrderStatusChangedToStockConfirmedIntegrationEvent>("/orderStatusChangedToStockConfirmed", features, eventBusOptions);
        api.MapSubscribe<OrderStatusChangedToShippedIntegrationEvent>("/orderStatusChangedToShipped", features, eventBusOptions);
        api.MapSubscribe<OrderStatusChangedToCancelledIntegrationEvent>("/orderStatusChangedToCancelled", features, eventBusOptions);

        if (features.Workflow.Enabled is false)
        {
            api.MapSubscribe<OrderStatusChangedToSubmittedIntegrationEvent>("/orderStatusChangedToSubmitted", features, eventBusOptions);
        }

        return api;
    }
}
