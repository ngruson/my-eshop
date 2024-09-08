using eShop.Shared.Data;

namespace eShop.Catalog.API.IntegrationEvents;

public interface ICatalogIntegrationEventService
{
    Task SaveEventAndDbChangesAsync(IRepository<CatalogItem> repository, IntegrationEvent evt, Func<Task>? func = null, CancellationToken cancellationToken = default);
    Task PublishThroughEventBusAsync(IntegrationEvent evt, CancellationToken cancellationToken);
}
