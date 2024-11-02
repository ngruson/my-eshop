using eShop.Shared.IntegrationEvents;
using Microsoft.EntityFrameworkCore.Storage;

namespace eShop.Ordering.API.Application.IntegrationEvents;

public class OrderingIntegrationEventService(IEventBus eventBus,
    OrderingContext orderingContext,
    IIntegrationEventLogService integrationEventLogService,
    ILogger<OrderingIntegrationEventService> logger) : IIntegrationEventService
{
    private readonly IEventBus _eventBus = eventBus;
    private readonly OrderingContext _orderingContext = orderingContext;
    private readonly IIntegrationEventLogService _eventLogService = integrationEventLogService;
    private readonly ILogger<OrderingIntegrationEventService> _logger = logger;

    public async Task PublishEventsThroughEventBusAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        IEnumerable<IntegrationEventLogEF.IntegrationEventLogEntry> pendingLogEvents =
            await this._eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId, cancellationToken);

        foreach (IntegrationEventLogEF.IntegrationEventLogEntry logEvt in pendingLogEvents)
        {
            this._logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", logEvt.EventId, logEvt.IntegrationEvent);

            try
            {
                await this._eventLogService.MarkEventAsInProgressAsync(logEvt.EventId, cancellationToken);
                await this._eventBus.PublishAsync(logEvt.IntegrationEvent!, cancellationToken);
                await this._eventLogService.MarkEventAsPublishedAsync(logEvt.EventId, cancellationToken);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error publishing integration event: {IntegrationEventId}", logEvt.EventId);

                await this._eventLogService.MarkEventAsFailedAsync(logEvt.EventId, cancellationToken);
            }
        }
    }

    public async Task AddAndSaveEventAsync(IntegrationEvent evt, CancellationToken cancellationToken)
    {
        IDbContextTransaction? transaction = this._orderingContext.GetCurrentTransaction();

        if (transaction is not null)
        {
            this._logger.LogInformation("Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

            await this._eventLogService.SaveEventAsync(evt, transaction.TransactionId, cancellationToken);
        }
    }
}
