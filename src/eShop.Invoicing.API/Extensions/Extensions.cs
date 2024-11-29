using eShop.EventBus.Dapr;
using eShop.EventBus.Extensions;
using eShop.EventBusRabbitMQ;
using eShop.Invoicing.API.Application.IntegrationEvents.EventHandling;
using eShop.Invoicing.API.Application.IntegrationEvents.Events;
using eShop.Invoicing.API.Application.Storage;
using eShop.Invoicing.API.Infrastructure;
using eShop.Ordering.Contracts;
using eShop.ServiceDefaults;
using eShop.ServiceInvocation;
using eShop.ServiceInvocation.OrderingApiClient;
using eShop.Shared.Features;
using Refit;

namespace eShop.Invoicing.API.Extensions;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultAuthentication();
        builder.AddClientCredentials(builder.Configuration);

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<Program>();
        });

        builder.Services.AddSingleton<IFileStorage, AzureBlobStorage>();

        FeaturesConfiguration? features = builder.Configuration.GetSection("Features").Get<FeaturesConfiguration>();
        if (features?.PublishSubscribe.EventBus == EventBusType.Dapr)
        {
            builder.AddDaprEventBus()
                .AddEventBusSubscriptions();
        }
        else
        {
            builder.AddRabbitMqEventBus("eventBus")
                .AddEventBusSubscriptions();
        }

        builder.AddServiceInvocation();
        if (features?.ServiceInvocation.ServiceInvocationType == ServiceInvocationType.Dapr)
        {
            builder.AddDaprServices();
        }
        else
        {
            builder.AddRefitServices();
        }
    }

    private static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
        eventBus.AddSubscription<OrderStatusChangedToStockConfirmedIntegrationEvent, OrderStatusChangedToStockConfirmedIntegrationEventHandler>();
    }

    private static void AddDaprServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IOrderingApiClient, ServiceInvocation.OrderingApiClient.Dapr.OrderingApiClient>();        
    }

    private static void AddRefitServices(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddRefitClient<IOrderingApi>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri("http://ordering-api"))
            .AddAuthToken();

        builder.Services.AddScoped<IOrderingApiClient, ServiceInvocation.OrderingApiClient.Refit.OrderingApiClient>();
    }

    private static void AddClientCredentials(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddClientCredentialsTokenManagement()
            .AddClient(configuration["Identity:ClientCredentials:ClientId"]!, client =>
            {
                client.TokenEndpoint = $"{configuration["Identity:Url"]}/connect/token"; //"https://demo.duendesoftware.com/connect/token";
                client.ClientId = configuration["Identity:ClientCredentials:ClientId"];
                client.ClientSecret = configuration["Identity:ClientCredentials:ClientSecret"];
                client.Scope = "orders";
            });
    }
}
