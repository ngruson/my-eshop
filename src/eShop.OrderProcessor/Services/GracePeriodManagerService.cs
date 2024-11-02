using eShop.EventBus.Abstractions;
using Microsoft.Extensions.Options;
using Npgsql;
using eShop.OrderProcessor.Events;

namespace eShop.OrderProcessor.Services;

public class GracePeriodManagerService(
    IOptions<BackgroundTaskOptions> options,
    IEventBus eventBus,
    ILogger<GracePeriodManagerService> logger,
    NpgsqlDataSource dataSource) : BackgroundService
{
    private readonly BackgroundTaskOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TimeSpan delayTime = TimeSpan.FromSeconds(this._options.CheckUpdateTime);

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("GracePeriodManagerService is starting.");
            stoppingToken.Register(() => logger.LogDebug("GracePeriodManagerService background task is stopping."));
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("GracePeriodManagerService background task is doing background work.");
            }

            await this.CheckConfirmedGracePeriodOrders();

            await Task.Delay(delayTime, stoppingToken);
        }

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("GracePeriodManagerService background task is stopping.");
        }
    }

    private async Task CheckConfirmedGracePeriodOrders()
    {
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Checking confirmed grace period orders");
        }

        List<int> orderIds = await this.GetConfirmedGracePeriodOrders();

        foreach (int orderId in orderIds)
        {
            GracePeriodConfirmedIntegrationEvent confirmGracePeriodEvent = new(orderId);

            logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", confirmGracePeriodEvent.Id, confirmGracePeriodEvent);

            await eventBus.PublishAsync(confirmGracePeriodEvent, default);
        }
    }

    private async ValueTask<List<int>> GetConfirmedGracePeriodOrders()
    {
        try
        {
            using NpgsqlConnection conn = dataSource.CreateConnection();
            using NpgsqlCommand command = conn.CreateCommand();
            command.CommandText = """
                SELECT "Id"
                FROM ordering.orders
                WHERE CURRENT_TIMESTAMP - "OrderDate" >= @GracePeriodTime AND "OrderStatus" = 'Submitted'
                """;
            command.Parameters.AddWithValue("GracePeriodTime", TimeSpan.FromMinutes(this._options.GracePeriodTime));

            List<int> ids = [];

            await conn.OpenAsync();
            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ids.Add(reader.GetInt32(0));
            }

            return ids;
        }
        catch (NpgsqlException exception)
        {
            logger.LogError(exception, "Fatal error establishing database connection");
        }

        return [];
    }
}
