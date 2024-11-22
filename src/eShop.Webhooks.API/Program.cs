using Asp.Versioning;
using Asp.Versioning.Builder;
using eShop.EventBus.Options;
using eShop.Shared.Features;
using eShop.Webhooks.API.Apis;
using eShop.Webhooks.API.Extensions;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();

IApiVersioningBuilder withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

FeaturesConfiguration? features = builder.Configuration.GetSection("Features").Get<FeaturesConfiguration>();
if (features?.PublishSubscribe.EventBus == EventBusType.Dapr)
{
    app.UseCloudEvents();
    app.MapSubscribeHandler();

    IOptions<EventBusOptions> eventbusOptions = app.Services.GetRequiredService<IOptions<EventBusOptions>>();
    app.MapSubscriptionEndpoints(eventbusOptions);
}

IVersionedEndpointRouteBuilder webHooks = app.NewVersionedApi("Web Hooks");

webHooks.MapWebHooksApiV1()
        .RequireAuthorization();

app.UseDefaultOpenApi();
app.Run();
