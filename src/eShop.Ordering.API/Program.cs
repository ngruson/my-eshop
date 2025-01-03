using Asp.Versioning;
using Asp.Versioning.Builder;
using eShop.EventBus.Options;
using eShop.Ordering.API.Apis;
using eShop.Shared.Features;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();

IApiVersioningBuilder withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

FeaturesConfiguration features = app.Services.GetRequiredService<IOptions<FeaturesConfiguration>>().Value;

IVersionedEndpointRouteBuilder orders = app.NewVersionedApi("Orders");

orders.MapOrdersApiV1(features.Workflow.Enabled)
    .RequireAuthorization();


if (features?.PublishSubscribe.EventBus == EventBusType.Dapr)
{
    app.UseCloudEvents();
    app.MapSubscribeHandler();

    EventBusOptions eventBusOptions = app.Services.GetRequiredService<IOptions<EventBusOptions>>().Value;
    app.MapSubscriptionEndpoints(features, eventBusOptions);
}

app.UseDefaultOpenApi();
app.Run();
