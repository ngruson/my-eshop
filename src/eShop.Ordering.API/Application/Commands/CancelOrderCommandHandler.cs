using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Commands;

// Regular CommandHandler
public class CancelOrderCommandHandler(IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository) : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> _orderRepository = orderRepository;

    /// <summary>
    /// Handler which processes the command when
    /// customer executes cancel order from app
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<bool> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var orderToUpdate = await this._orderRepository.GetByIdAsync(command.OrderNumber, cancellationToken);

        if (orderToUpdate is null)
        {
            return false;
        }

        orderToUpdate.SetCancelledStatus();
        await this._orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
        return true;
    }
}

// Use for Idempotency in Command process
public class CancelOrderIdentifiedCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<CancelOrderCommand, bool>> logger) : IdentifiedCommandHandler<CancelOrderCommand, bool>(mediator, requestManager, logger)
{
    protected override bool CreateResultForDuplicateRequest()
    {
        return true; // Ignore duplicate requests for processing order.
    }
}
