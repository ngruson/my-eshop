using eShop.IntegrationEventLogEF.Specifications;
using eShop.Shared.Data;

namespace eShop.IntegrationEventLogEF.Services;

public class IntegrationEventLogService : IIntegrationEventLogService
{
    private readonly IRepository<IntegrationEventLogEntry> _repository;
    private Type[] _eventTypes = [];

    public IntegrationEventLogService(IRepository<IntegrationEventLogEntry> repository)
    {
        this._repository = repository;
        this.LoadEventTypes(Assembly.GetEntryAssembly()!);
    }

    public void LoadEventTypes(Assembly assembly)
    {
        this._eventTypes = Assembly.Load(assembly.FullName!)
            .GetTypes()
            .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
            .ToArray();
    }

    public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        List<IntegrationEventLogEntry> result = await this._repository.ListAsync(
            new GetPendingEventLogsSpecification(transactionId, this._eventTypes), cancellationToken);

        if (result.Count > 0)
        {
            return result
                .Select(e => e.DeserializeJsonContent(this._eventTypes.FirstOrDefault(t => t.Name == e.EventTypeShortName)!))
                .ToList();
        }

        return [];
    }

    public async Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        var eventLogEntry = new IntegrationEventLogEntry(@event, transaction.TransactionId);

        await this._repository.AddAsync(eventLogEntry, cancellationToken);
    }

    public Task MarkEventAsPublishedAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return this.UpdateEventStatus(eventId, EventStateEnum.Published);
    }

    public Task MarkEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return this.UpdateEventStatus(eventId, EventStateEnum.InProgress);
    }

    public Task MarkEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return this.UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
    }

    private async Task UpdateEventStatus(Guid eventId, EventStateEnum status)
    {
        var eventLogEntry = await this._repository.SingleOrDefaultAsync(new GetEventSpecification(eventId));

        if (eventLogEntry != null)
        {
            eventLogEntry.State = status;

            if (status == EventStateEnum.InProgress)
                eventLogEntry.TimesSent++;

            await this._repository.UpdateAsync(eventLogEntry);
        }

        await this._repository.SaveChangesAsync();
    }
}
