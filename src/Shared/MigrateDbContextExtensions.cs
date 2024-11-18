using System.Diagnostics;
using eShop.Shared.Data.Seed;
using eShop.Shared.DI;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.AspNetCore.Hosting;

internal static class MigrateDbContextExtensions
{
    private static readonly string ActivitySourceName = "DbMigrations";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services, IConfiguration configuration)
        where TContext : DbContext
        => services.AddMigration<TContext>(configuration, Array.Empty<Type>());

    private static IServiceCollection AddMigration<TContext>(this IServiceCollection services, IConfiguration configuration, params Func<ServiceProviderWrapper, IDbSeeder>[] seederFactories)
        where TContext : DbContext
    {
        // Enable migration tracing
        services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource(ActivitySourceName));

        return services.AddHostedService(sp => new MigrationHostedService<TContext>(sp, configuration, seederFactories));
    }

    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services, IConfiguration configuration, params Type[] seederTypes)
        where TContext : DbContext
    {
        List<Func<ServiceProviderWrapper, IDbSeeder>> seederFactories = [];

        foreach (Type seederType in seederTypes)
        {
            if (!typeof(IDbSeeder).IsAssignableFrom(seederType))
            {
                throw new ArgumentException($"Type {seederType.Name} does not implement IDbSeeder interface.");
            }

            services.AddScoped(seederType);
            seederFactories.Add(sp => (IDbSeeder)sp.GetRequiredService(seederType));
        }

        return services.AddMigration<TContext>(configuration, seederFactories.ToArray());
    }

    private static async Task MigrateDbContextAsync<TContext>(this IServiceProvider services, IConfiguration configuration, params Func<ServiceProviderWrapper, IDbSeeder>[] seederFactories) where TContext : DbContext
    {
        using IServiceScope scope = services.CreateScope();
        IServiceProvider scopeServices = scope.ServiceProvider;
        ILogger<TContext> logger = scopeServices.GetRequiredService<ILogger<TContext>>();
        TContext context = scopeServices.GetRequiredService<TContext>();

        using Activity? activity = ActivitySource.StartActivity($"Migration operation {typeof(TContext).Name}");

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

            IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();

            ServiceProviderWrapper serviceProviderWrapper =
                new(scopeServices);

            await strategy.ExecuteAsync(() => InvokeSeeders(context, serviceProviderWrapper, configuration, seederFactories));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

            activity?.SetExceptionTags(ex);

            throw;
        }
    }

    private static async Task InvokeSeeders<TContext>(TContext context, ServiceProviderWrapper services, IConfiguration configuration, params Func<ServiceProviderWrapper, IDbSeeder>[] seederFactories)
        where TContext : DbContext
    {
        using Activity? activity = ActivitySource.StartActivity($"Migrating {typeof(TContext).Name}");

        try
        {
            bool useMigrations = configuration.GetValue("UseMigrations", false);
            if (useMigrations)
            {
                await context.Database.MigrateAsync();
            }
            else
            {
                await context.Database.EnsureCreatedAsync();
            }

            foreach (Func<ServiceProviderWrapper, IDbSeeder> seederFactory in seederFactories)
            {
                IDbSeeder seeder = seederFactory(services);
                await seeder.SeedAsync(services);
            }
        }
        catch (Exception ex)
        {
            activity?.SetExceptionTags(ex);

            throw;
        }
    }
    
    private class MigrationHostedService<TContext>(IServiceProvider serviceProvider, IConfiguration configuration, params Func<ServiceProviderWrapper, IDbSeeder>[] seederFactories)
        : BackgroundService where TContext : DbContext
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return serviceProvider.MigrateDbContextAsync<TContext>(configuration, seederFactories);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
