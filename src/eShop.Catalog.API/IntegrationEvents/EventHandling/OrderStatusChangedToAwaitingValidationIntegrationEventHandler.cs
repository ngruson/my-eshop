using eShop.Catalog.API.Application.Commands.AssessStockItemsForOrder;
using MediatR;

namespace eShop.Catalog.API.IntegrationEvents.EventHandling;

public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger)
        : IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToAwaitingValidationIntegrationEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        await mediator.Send(new AssessStockItemsForOrderCommand(
            new Contracts.AssessStockItemsForOrder.AssessStockItemsForOrderRequestDto(
                @event.OrderId,
                [.. @event.OrderStockItems.Select(_ => new Contracts.AssessStockItemsForOrder.OrderStockItem(_.ProductId, _.Units))])),
        cancellationToken);
    }
}
