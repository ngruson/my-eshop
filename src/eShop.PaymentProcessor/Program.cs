using eShop.EventBus.Extensions;
using eShop.EventBus.Dapr;
using eShop.EventBus.Options;
using eShop.EventBusRabbitMQ;
using eShop.PaymentProcessor.Extensions;
using eShop.Shared.Features;
using Asp.Versioning.Builder;
using Asp.Versioning;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultAuthentication();

FeaturesConfiguration? features = builder.Configuration.GetSection("Features").Get<FeaturesConfiguration>();

if (features!.Workflow.Enabled is false)
{
    if (features?.PublishSubscribe.EventBus == EventBusType.Dapr)
    {
        builder.AddDaprEventBus()
            .AddSubscription<OrderStatusChangedToStockConfirmedIntegrationEvent, OrderStatusChangedToStockConfirmedIntegrationEventHandler>();
    }
    else
    {
        builder.AddRabbitMqEventBus("eventBus")
            .AddSubscription<OrderStatusChangedToStockConfirmedIntegrationEvent, OrderStatusChangedToStockConfirmedIntegrationEventHandler>();
    }
}

builder.Services.AddOptions<PaymentOptions>()
    .BindConfiguration(nameof(PaymentOptions));

IApiVersioningBuilder withApiVersioning = builder.Services.AddApiVersioning();
builder.AddDefaultOpenApi(withApiVersioning);

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

if (features!.PublishSubscribe.EventBus == EventBusType.Dapr && features.Workflow.Enabled is false)
{
    app.UseCloudEvents();
    app.MapSubscribeHandler();

    IOptions<EventBusOptions> eventBusOptions = app.Services.GetRequiredService<IOptions<EventBusOptions>>();
    app.MapSubscriptionEndpoints(features, eventBusOptions.Value);
}

if (features.Workflow.Enabled)
{
    IVersionedEndpointRouteBuilder workflow = app.NewVersionedApi("Workflow");
    workflow.MapApiV1()
        .RequireAuthorization();
}

await app.RunAsync();
