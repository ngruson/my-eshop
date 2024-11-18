using eShop.EventBus.Abstractions;
using eShop.EventBus.Extensions;
using eShop.EventBus.Options;
using Google.Api;
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
}
