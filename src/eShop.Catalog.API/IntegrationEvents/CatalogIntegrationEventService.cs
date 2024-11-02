using eShop.Shared.IntegrationEvents;
using Microsoft.EntityFrameworkCore.Storage;

namespace eShop.Catalog.API.IntegrationEvents;

public sealed class CatalogIntegrationEventService(
    IEventBus eventBus,
    IIntegrationEventLogService integrationEventLogService,
    CatalogContext dbContext,
    ILogger<CatalogIntegrationEventService> logger) : IIntegrationEventService
{
    public async Task AddAndSaveEventAsync(IntegrationEvent evt, CancellationToken cancellationToken = default)
    {
        IDbContextTransaction? transaction = dbContext.GetCurrentTransaction();

        if (transaction is not null)
        {
            logger.LogInformation("Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

            await integrationEventLogService.SaveEventAsync(evt, transaction.TransactionId, cancellationToken);
        }
    }

    public async Task PublishEventsThroughEventBusAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        IEnumerable<IntegrationEventLogEntry> pendingLogEvents =
            await integrationEventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId, cancellationToken);

        foreach (IntegrationEventLogEntry logEvt in pendingLogEvents)
        {
            logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", logEvt.EventId, logEvt.IntegrationEvent);

            try
            {
                await integrationEventLogService.MarkEventAsInProgressAsync(logEvt.EventId, cancellationToken);
                await eventBus.PublishAsync(logEvt.IntegrationEvent!, cancellationToken);
                await integrationEventLogService.MarkEventAsPublishedAsync(logEvt.EventId, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error publishing integration event: {IntegrationEventId}", logEvt.EventId);

                await integrationEventLogService.MarkEventAsFailedAsync(logEvt.EventId, cancellationToken);
            }
        }
    }
}
