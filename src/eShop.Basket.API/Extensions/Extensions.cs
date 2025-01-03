using System.Text.Json.Serialization;
using eShop.Basket.API.Repositories;
using eShop.Basket.API.IntegrationEvents.EventHandling;
using eShop.EventBus.Dapr;
using eShop.EventBus.Extensions;
using eShop.EventBusRabbitMQ;
using eShop.Basket.API.IntegrationEvents.Events;
using eShop.Shared.Features;

namespace eShop.Basket.API.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultAuthentication();
        builder.AddRedisClient("redis");

        builder.Services.Configure<FeaturesConfiguration>(builder.Configuration.GetSection("Features"));
        builder.Services.AddSingleton<IBasketRepository, DaprBasketRepository>();

        FeaturesConfiguration? features = builder.Configuration.GetSection("Features").Get<FeaturesConfiguration>();
        if (features!.PublishSubscribe.EventBus == EventBusType.Dapr)
        {
            builder.AddDaprEventBus()
                .AddEventBusSubscriptions(features.Workflow.Enabled)
                .ConfigureJsonOptions(options => options.TypeInfoResolverChain.Add(IntegrationEventContext.Default));
        }
        else
        {
            builder.AddRabbitMqEventBus("eventBus")
                .AddEventBusSubscriptions(features.Workflow.Enabled)
                .ConfigureJsonOptions(options => options.TypeInfoResolverChain.Add(IntegrationEventContext.Default));
        }
    }

    private static IEventBusBuilder AddEventBusSubscriptions(this IEventBusBuilder eventBus, bool workflowEnabled)
    {
        if (workflowEnabled is false)
        {
            eventBus.AddSubscription<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();
        }
        
        return eventBus;
    }
}

[JsonSerializable(typeof(OrderStartedIntegrationEvent))]
partial class IntegrationEventContext : JsonSerializerContext
{

}
