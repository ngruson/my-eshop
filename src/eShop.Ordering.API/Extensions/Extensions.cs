using Ardalis.Result;
using eShop.EventBus.Dapr;
using eShop.EventBusRabbitMQ;
using eShop.Ordering.API.Application.Commands.CancelOrder;
using eShop.Ordering.API.Application.Commands.CreateOrder;
using eShop.Ordering.API.Application.Commands.ShipOrder;
using eShop.Ordering.Infrastructure.Repositories;
using eShop.Shared.Behaviors;
using eShop.Shared.Data;
using eShop.Shared.Data.EntityFramework;
using eShop.Shared.Features;
using eShop.Shared.IntegrationEvents;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<FeaturesConfiguration>(builder.Configuration.GetSection("Features"));
        FeaturesConfiguration? features = builder.Configuration.GetSection("Features").Get<FeaturesConfiguration>();

        // Add the authentication services to DI
        builder.AddDefaultAuthentication();

        // Pooling is disabled because of the following error:
        // Unhandled exception. System.InvalidOperationException:
        // The DbContext of type 'OrderingContext' cannot be pooled because it does not have a public constructor accepting a single parameter of type DbContextOptions or has more than one constructor.
        builder.Services.AddDbContext<OrderingContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("orderingDb"));
        });
        builder.Services.AddScoped<eShopDbContext>(sp => sp.GetRequiredService<OrderingContext>());
        builder.EnrichNpgsqlDbContext<OrderingContext>();

        builder.Services.AddMigration<OrderingContext>(builder.Configuration, typeof(CardTypesSeed));

        // Add the integration services that consume the DbContext
        builder.Services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService>();

        builder.Services.AddTransient<IIntegrationEventService, OrderingIntegrationEventService>();

        
        if (features!.PublishSubscribe.EventBus == EventBusType.Dapr)
        {
            builder.AddDaprEventBus()
                .AddEventBusSubscriptions(features.Workflow.Enabled);
        }
        else
        {
            builder.AddRabbitMqEventBus("eventBus")
                .AddEventBusSubscriptions(features.Workflow.Enabled);
        }

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        // Configure Mediator
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<Program>();

            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        // Register the command validators for the validator behavior (validators based on FluentValidation library)
        builder.Services.AddSingleton<IValidator<CancelOrderCommand>, CancelOrderCommandValidator>();
        builder.Services.AddSingleton<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
        builder.Services.AddSingleton<IValidator<IdentifiedCommand<CreateOrderCommand, Result<Guid>>>, IdentifiedCommandValidator>();
        builder.Services.AddSingleton<IValidator<ShipOrderCommand>, ShipOrderCommandValidator>();

        builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        builder.Services.AddScoped<IRequestManager, RequestManager>();
    }

    private static void AddEventBusSubscriptions(this IEventBusBuilder eventBus, bool workflowEnabled)
    {
        if (workflowEnabled is false)
        {
            eventBus.AddSubscription<GracePeriodConfirmedIntegrationEvent, GracePeriodConfirmedIntegrationEventHandler>();
            eventBus.AddSubscription<OrderStockConfirmedIntegrationEvent, OrderStockConfirmedIntegrationEventHandler>();
            eventBus.AddSubscription<OrderStockRejectedIntegrationEvent, OrderStockRejectedIntegrationEventHandler>();
        }
        
        eventBus.AddSubscription<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();
        eventBus.AddSubscription<OrderPaymentSucceededIntegrationEvent, OrderPaymentSucceededIntegrationEventHandler>();
    }
}
