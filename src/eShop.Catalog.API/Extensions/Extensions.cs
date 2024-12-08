using eShop.Catalog.API.Services;
using eShop.EventBus.Dapr;
using eShop.EventBus.Extensions;
using eShop.EventBusRabbitMQ;
using eShop.Shared.Behaviors;
using eShop.Shared.Data;
using eShop.Shared.Data.EntityFramework;
using eShop.Shared.Features;
using eShop.Shared.IntegrationEvents;
using Microsoft.SemanticKernel;

namespace eShop.Catalog.API.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<FeaturesConfiguration>(builder.Configuration.GetSection("Features"));

        builder.AddNpgsqlDbContext<CatalogContext>("catalogdb", configureDbContextOptions: dbContextOptionsBuilder =>
        {
            dbContextOptionsBuilder.UseNpgsql(builder =>
            {
                builder.UseVector();
            });
        });
        builder.Services.AddScoped<eShopDbContext>(sp => sp.GetRequiredService<CatalogContext>());

        // REVIEW: This is done for development ease but shouldn't be here in production
        builder.Services.AddMigration<CatalogContext>(builder.Configuration, typeof(CatalogSeed));
        builder.Services.AddScoped<CatalogSeed>();

        builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        // Add the integration services that consume the DbContext
        builder.Services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService>();
        builder.Services.AddTransient<IIntegrationEventService, CatalogIntegrationEventService>();

        // Configure Mediator
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<Program>();

            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        FeaturesConfiguration? features = builder.Configuration.GetSection("Features").Get<FeaturesConfiguration>();
        if (features?.PublishSubscribe.EventBus == EventBusType.Dapr)
        {
            builder.AddDaprEventBus()
                .AddEventBusSubscriptions()
                .ConfigureJsonOptions(options => options.PropertyNameCaseInsensitive = true);
        }
        else
        {
            builder.AddRabbitMqEventBus("eventBus")
                .AddEventBusSubscriptions()
                .ConfigureJsonOptions(options => options.PropertyNameCaseInsensitive = true);
        }

        builder.Services.AddOptions<CatalogOptions>()
            .BindConfiguration(nameof(CatalogOptions));

        if (builder.Configuration["AI:Onnx:EmbeddingModelPath"] is string modelPath &&
            builder.Configuration["AI:Onnx:EmbeddingVocabPath"] is string vocabPath)
        {
            builder.Services.AddBertOnnxTextEmbeddingGeneration(modelPath, vocabPath);
        }
        else if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("openai")))
        {
            builder.AddAzureOpenAIClient("openai");
            builder.Services.AddOpenAITextEmbeddingGeneration(builder.Configuration["AIOptions:OpenAI:EmbeddingName"] ?? "text-embedding-3-small");
        }

        builder.Services.AddSingleton<TextEmbeddingGenerationServiceWrapper>();
        builder.Services.AddSingleton<ICatalogAI, CatalogAI>();
    }

    private static IEventBusBuilder AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
        eventBus.AddSubscription<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
        eventBus.AddSubscription<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();

        return eventBus;
    }
}
