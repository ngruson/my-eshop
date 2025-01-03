using eShop.Catalog.API.APIs;
using eShop.Catalog.API.Extensions;
using eShop.EventBus.Options;
using eShop.Shared.Features;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();

IApiVersioningBuilder withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

FeaturesConfiguration features = app.Services.GetRequiredService<IOptions<FeaturesConfiguration>>().Value;
if (features.PublishSubscribe.EventBus == EventBusType.Dapr)
{
    app.UseCloudEvents();
    app.MapSubscribeHandler();

    EventBusOptions eventBusOptions = app.Services.GetRequiredService<IOptions<EventBusOptions>>().Value;
    app.MapSubscriptionEndpoints(features, eventBusOptions);
}

app.NewVersionedApi("Catalog")
   .MapCatalogApiV1(features.Workflow.Enabled);

app.UseDefaultOpenApi();
app.Run();
