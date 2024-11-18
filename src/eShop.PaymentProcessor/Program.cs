using eShop.EventBus.Extensions;
using eShop.EventBusRabbitMQ;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRabbitMqEventBus("EventBus")
    .AddSubscription<OrderStatusChangedToStockConfirmedIntegrationEvent, OrderStatusChangedToStockConfirmedIntegrationEventHandler>();

builder.Services.AddOptions<PaymentOptions>()
    .BindConfiguration(nameof(PaymentOptions));

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

await app.RunAsync();
