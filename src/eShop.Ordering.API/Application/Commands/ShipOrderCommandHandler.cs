using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Commands;

// Regular CommandHandler
public class ShipOrderCommandHandler(IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository) : IRequestHandler<ShipOrderCommand, bool>
{
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> _orderRepository = orderRepository;

    /// <summary>
    /// Handler which processes the command when
    /// administrator executes ship order from app
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<bool> Handle(ShipOrderCommand command, CancellationToken cancellationToken)
    {
        Domain.AggregatesModel.OrderAggregate.Order? orderToUpdate = await this._orderRepository.GetByIdAsync(command.OrderNumber, cancellationToken);
        if (orderToUpdate is null)
        {
            return false;
        }

        orderToUpdate.SetShippedStatus();
        await this._orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
        return true;
    }
}

// Use for Idempotency in Command process
public class ShipOrderIdentifiedCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<ShipOrderCommand, bool>> logger) : IdentifiedCommandHandler<ShipOrderCommand, bool>(mediator, requestManager, logger)
{
    protected override bool CreateResultForDuplicateRequest()
    {
        return true; // Ignore duplicate requests for processing order.
    }
}
