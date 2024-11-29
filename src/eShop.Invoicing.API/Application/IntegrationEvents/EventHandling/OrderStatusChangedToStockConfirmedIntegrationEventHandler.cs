using eShop.EventBus.Abstractions;
using eShop.EventBus.Extensions;
using eShop.Invoicing.API.Application.Commands.CreateInvoice;
using eShop.Invoicing.API.Application.IntegrationEvents.Events;
using MediatR;

namespace eShop.Invoicing.API.Application.IntegrationEvents.EventHandling;

public class OrderStatusChangedToStockConfirmedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderStatusChangedToStockConfirmedIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToStockConfirmedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        CreateInvoiceCommand command = new(@event.OrderId);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderId),
            command.OrderId,
            command);

        await mediator.Send(command, cancellationToken);
    }
}
