using eShop.Shared.Data;

namespace eShop.Catalog.API.IntegrationEvents.EventHandling;

public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
    IRepository<CatalogItem> repository,
    ICatalogIntegrationEventService catalogIntegrationEventService,
    ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToAwaitingValidationIntegrationEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        List<ConfirmedOrderStockItem> confirmedOrderStockItems = [];

        foreach (OrderStockItem orderStockItem in @event.OrderStockItems)
        {
            CatalogItem? catalogItem = await repository.GetByIdAsync(orderStockItem.ProductId, cancellationToken);
            bool hasStock = catalogItem!.AvailableStock >= orderStockItem.Units;
            ConfirmedOrderStockItem confirmedOrderStockItem = new(catalogItem.Id, hasStock);

            confirmedOrderStockItems.Add(confirmedOrderStockItem);
        }

        IntegrationEvent confirmedIntegrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
            ? new OrderStockRejectedIntegrationEvent(@event.OrderId, confirmedOrderStockItems)
            : new OrderStockConfirmedIntegrationEvent(@event.OrderId);

        await catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(confirmedIntegrationEvent, cancellationToken);
        await catalogIntegrationEventService.PublishThroughEventBusAsync(confirmedIntegrationEvent, cancellationToken);
    }
}
