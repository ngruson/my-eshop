using Asp.Versioning;
using Asp.Versioning.Builder;
using eShop.EventBus.Options;
using eShop.Invoicing.API;
using eShop.Invoicing.API.Extensions;
using eShop.ServiceDefaults;
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

IVersionedEndpointRouteBuilder orders = app.NewVersionedApi("Invoices");

orders.MapInvoiceApiV1()
    .RequireAuthorization();

FeaturesConfiguration features = app.Services.GetRequiredService<IOptions<FeaturesConfiguration>>().Value;
if (features?.PublishSubscribe.EventBus == EventBusType.Dapr)
{
    app.UseCloudEvents();
    app.MapSubscribeHandler();

    IOptions<EventBusOptions> eventBusOptions = app.Services.GetRequiredService<IOptions<EventBusOptions>>();
    app.MapSubscriptionEndpoints(features, eventBusOptions.Value);
}

app.UseDefaultOpenApi();
app.Run();
