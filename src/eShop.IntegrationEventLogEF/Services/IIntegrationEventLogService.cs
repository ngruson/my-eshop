namespace eShop.IntegrationEventLogEF.Services;

public interface IIntegrationEventLogService
{
    Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId, CancellationToken cancellationToken);
    Task SaveEventAsync(IntegrationEvent @event, Guid transactionId, CancellationToken cancellationToken);
    Task MarkEventAsPublishedAsync(Guid eventId, CancellationToken cancellationToken);
    Task MarkEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken);
    Task MarkEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken);
}
