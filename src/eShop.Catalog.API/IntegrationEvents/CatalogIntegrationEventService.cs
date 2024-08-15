using eShop.Shared.Data;

namespace eShop.Catalog.API.IntegrationEvents;

public sealed class CatalogIntegrationEventService(ILogger<CatalogIntegrationEventService> logger,
    IEventBus eventBus,
    //CatalogContext catalogContext,
    IIntegrationEventLogService integrationEventLogService)
        : ICatalogIntegrationEventService
{
    public async Task PublishThroughEventBusAsync(IntegrationEvent evt, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Publishing integration event: {IntegrationEventId_published} - ({@IntegrationEvent})", evt.Id, evt);

            await integrationEventLogService.MarkEventAsInProgressAsync(evt.Id, cancellationToken);
            await eventBus.PublishAsync(evt, cancellationToken);
            await integrationEventLogService.MarkEventAsPublishedAsync(evt.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", evt.Id, evt);
            await integrationEventLogService.MarkEventAsFailedAsync(evt.Id, cancellationToken);
        }
    }

    public async Task SaveEventAndDbChangesAsync(IRepository<CatalogItem> repository, IntegrationEvent evt, Func<Task>? func, CancellationToken cancellationToken)
    {
        logger.LogInformation("CatalogIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

        await repository.ExecuteInTransactionAsync(async (Guid transactionId) =>
            {
                // Achieving atomicity between original catalog database operation and the IntegrationEventLog thanks to a local transaction
                if (func is not null)
                {
                    await func();
                }
                await integrationEventLogService.SaveEventAsync(evt, transactionId, cancellationToken);
            },
            cancellationToken);
    }
}
