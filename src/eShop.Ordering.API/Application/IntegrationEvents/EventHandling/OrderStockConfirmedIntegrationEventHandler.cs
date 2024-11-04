using eShop.Ordering.API.Application.Commands.SetStockConfirmedOrderStatus;

namespace eShop.Ordering.API.Application.IntegrationEvents.EventHandling;

public class OrderStockConfirmedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderStockConfirmedIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>
{
    public async Task Handle(OrderStockConfirmedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        SetStockConfirmedOrderStatusCommand command = new(@event.OrderId);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command, cancellationToken);
    }
}
