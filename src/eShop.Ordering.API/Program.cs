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

IVersionedEndpointRouteBuilder orders = app.NewVersionedApi("Orders");

orders.MapOrdersApiV1()
      .RequireAuthorization();

FeaturesConfiguration? features = builder.Configuration.GetSection("Features").Get<FeaturesConfiguration>();
if (features?.PublishSubscribe.EventBus == EventBusType.Dapr)
{
    //app.UseCloudEvents();
    app.MapSubscribeHandler();

    IOptions<EventBusOptions> eventbusOptions = app.Services.GetRequiredService<IOptions<EventBusOptions>>();
    app.MapSubscriptionEndpoints(eventbusOptions);
}

app.UseDefaultOpenApi();
app.Run();
