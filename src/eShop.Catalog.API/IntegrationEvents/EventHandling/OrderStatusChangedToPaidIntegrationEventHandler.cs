using eShop.Catalog.API.Specifications;
using eShop.Shared.Data;

namespace eShop.Catalog.API.IntegrationEvents.EventHandling;

public class OrderStatusChangedToPaidIntegrationEventHandler(
    IRepository<CatalogItem> repository,
    ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToPaidIntegrationEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        //we're not blocking stock/inventory
        foreach (OrderStockItem orderStockItem in @event.OrderStockItems)
        {
            CatalogItem? catalogItem = await repository.SingleOrDefaultAsync(
                new GetCatalogItemByObjectIdSpecification(orderStockItem.ProductId),
                cancellationToken);

            catalogItem!.RemoveStock(orderStockItem.Units);

            await repository.UpdateAsync(catalogItem!, cancellationToken);
        }
    }
}
