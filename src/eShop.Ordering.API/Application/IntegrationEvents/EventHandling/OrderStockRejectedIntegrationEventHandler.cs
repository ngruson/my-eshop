using eShop.Ordering.API.Application.Commands.SetStockRejectedOrderStatus;

namespace eShop.Ordering.API.Application.IntegrationEvents.EventHandling;
public class OrderStockRejectedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderStockRejectedIntegrationEventHandler> logger) : IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>
{
    public async Task Handle(OrderStockRejectedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        Guid[] orderStockRejectedItems = [.. @event.OrderStockItems
            .FindAll(c => !c.HasStock)
            .Select(c => c.ProductId)];

        SetStockRejectedOrderStatusCommand command = new(@event.OrderId, orderStockRejectedItems);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.ObjectId),
            command.ObjectId,
            command);

        await mediator.Send(command, cancellationToken);
    }
}
