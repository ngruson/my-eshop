namespace eShop.Catalog.API.IntegrationEvents;

public sealed class CatalogIntegrationEventService(ILogger<CatalogIntegrationEventService> logger,
    IEventBus eventBus,
    CatalogContext catalogContext,
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

    public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt, CancellationToken cancellationToken)
    {
        logger.LogInformation("CatalogIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

        //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
        await ResilientTransaction.New(catalogContext).ExecuteAsync(async () =>
        {
            // Achieving atomicity between original catalog database operation and the IntegrationEventLog thanks to a local transaction
            await catalogContext.SaveChangesAsync();
            await integrationEventLogService.SaveEventAsync(evt, catalogContext.Database.CurrentTransaction!, cancellationToken);
        });
    }
}
