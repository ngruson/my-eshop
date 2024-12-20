using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Commands.SetStockRejectedOrderStatus;

// Regular CommandHandler
public class SetStockRejectedOrderStatusCommandHandler(IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository) : IRequestHandler<SetStockRejectedOrderStatusCommand, bool>
{
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> _orderRepository = orderRepository;

    /// <summary>
    /// Handler which processes the command when
    /// Stock service rejects the request
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<bool> Handle(SetStockRejectedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        // Simulate a work time for rejecting the stock
        await Task.Delay(10000, cancellationToken);

        Domain.AggregatesModel.OrderAggregate.Order? orderToUpdate =
            await this._orderRepository.SingleOrDefaultAsync(new GetOrderSpecification(command.ObjectId), cancellationToken);

        if (orderToUpdate is null)
        {
            return false;
        }

        orderToUpdate.SetCancelledStatusWhenStockIsRejected(command.OrderStockItems);

        await this._orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
        return true;
    }
}


// Use for Idempotency in Command process
public class SetStockRejectedOrderStatusIdentifiedCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<SetStockRejectedOrderStatusCommand, bool>> logger) : IdentifiedCommandHandler<SetStockRejectedOrderStatusCommand, bool>(mediator, requestManager, logger)
{
    protected override bool CreateResultForDuplicateRequest()
    {
        return true; // Ignore duplicate requests for processing order.
    }
}
