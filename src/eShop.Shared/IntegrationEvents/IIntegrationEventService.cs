using eShop.EventBus.Events;

namespace eShop.Shared.IntegrationEvents;

public interface IIntegrationEventService
{
    Task PublishEventsThroughEventBusAsync(Guid transactionId, CancellationToken cancellationToken = default);
    Task AddAndSaveEventAsync(IntegrationEvent evt, CancellationToken cancellationToken = default);
}
