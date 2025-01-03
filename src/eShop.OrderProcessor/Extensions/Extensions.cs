using System.Text.Json.Serialization;
using eShop.EventBus.Dapr;
using eShop.EventBus.Extensions;
using eShop.EventBusRabbitMQ;
using eShop.OrderProcessor.Events;
using eShop.ServiceInvocation;
using eShop.ServiceInvocation.WorkflowApiClient;
using eShop.Shared.Features;
using eShop.Workflow.Contracts;
using Refit;

namespace eShop.OrderProcessor.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddClientCredentials(builder.Configuration, "workflows");

        IConfigurationSection featuresSection = builder.Configuration.GetSection("Features");
        builder.Services.Configure<FeaturesConfiguration>(featuresSection);

        FeaturesConfiguration? features = featuresSection.Get<FeaturesConfiguration>();
        if (features?.PublishSubscribe.EventBus == EventBusType.Dapr)
        {
            builder.AddDaprEventBus()
                .ConfigureJsonOptions(options => options.TypeInfoResolverChain.Add(IntegrationEventContext.Default));
        }
        else
        {
            builder.AddRabbitMqEventBus("eventBus")
                .ConfigureJsonOptions(options => options.TypeInfoResolverChain.Add(IntegrationEventContext.Default));
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

        builder.AddNpgsqlDataSource("orderingdb");

        builder.Services.AddOptions<BackgroundTaskOptions>()
            .BindConfiguration(nameof(BackgroundTaskOptions));

        builder.Services.AddHostedService<GracePeriodManagerService>();
    }

    private static void AddDaprServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDaprClient();        
        builder.Services.AddSingleton<IWorkflowApiClient, ServiceInvocation.WorkflowApiClient.Dapr.WorkflowApiClient>();
    }

    private static void AddRefitServices(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddRefitClient<IWorkflowApi>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri("http://workflow-api"))
            .AddAuthToken();
        
        builder.Services.AddSingleton<IWorkflowApiClient, ServiceInvocation.WorkflowApiClient.Refit.WorkflowApiClient>();
    }
}

[JsonSerializable(typeof(GracePeriodConfirmedIntegrationEvent))]
partial class IntegrationEventContext : JsonSerializerContext
{

}
