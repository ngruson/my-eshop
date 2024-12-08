using Dapr;
using eShop.EventBus.Abstractions;
using eShop.EventBus.Events;
using eShop.EventBus.Extensions;
using eShop.EventBus.Options;
using eShop.Shared.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eShop.EventBus.Dapr;

public static class DependencyInjectionExtensions
{
    private const string SectionName = "EventBus";

    public static IEventBusBuilder AddDaprEventBus(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<EventBusOptions>(builder.Configuration.GetSection(SectionName));
        builder.Services.AddSingleton<IEventBus, DaprEventBus>();
        builder.Services.AddDaprClient();

        return new EventBusBuilder(builder.Services);
    }

    public static void MapSubscribe<T>(this RouteGroupBuilder routeGroupBuilder, string path, FeaturesConfiguration features, EventBusOptions options)
        where T : IntegrationEvent
    {
        routeGroupBuilder.MapPost(path, async (
            T integrationEvent,
            [FromServices] IServiceProvider serviceProvider) =>
        {
            IIntegrationEventHandler eventHandler =
                serviceProvider.GetRequiredKeyedService<IIntegrationEventHandler>(
                    typeof(T));

            await eventHandler.Handle(integrationEvent, default);
        })
        .WithTopic(new TopicOptions
        {
            Match = $"event.data.messageType == \"{typeof(T).Name}\"",
            Metadata = new Dictionary<string, string>()
            {
                { "queueName", options.SubscriptionClientName },
                { "routingKey", typeof(T).Name }
            },
            Name = features.PublishSubscribe.TopicName,
            PubsubName = features.PublishSubscribe.PubsubName
        });
    }
}
