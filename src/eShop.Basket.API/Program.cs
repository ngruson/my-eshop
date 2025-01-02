using eShop.EventBus.Options;
using eShop.Shared.Features;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddBasicServiceDefaults();
builder.AddApplicationServices();

builder.Services.AddGrpc();

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

app.MapGrpcService<BasketService>();

FeaturesConfiguration? features = builder.Configuration.GetSection("Features").Get<FeaturesConfiguration>();
if (features!.PublishSubscribe.EventBus == EventBusType.Dapr && features.Workflow.Enabled is false)
{
    ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Configuring Dapr EventBus...");

    app.UseCloudEvents();
    app.MapSubscribeHandler();

    IOptions<EventBusOptions> eventBusOptions = app.Services.GetRequiredService<IOptions<EventBusOptions>>();
    app.MapSubscriptionEndpoints(features, eventBusOptions.Value);
}

app.Run();
