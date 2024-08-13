namespace eShop.Catalog.API.IntegrationEvents;

public interface ICatalogIntegrationEventService
{
    Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt, CancellationToken cancellationToken);
    Task PublishThroughEventBusAsync(IntegrationEvent evt, CancellationToken cancellationToken);
}
