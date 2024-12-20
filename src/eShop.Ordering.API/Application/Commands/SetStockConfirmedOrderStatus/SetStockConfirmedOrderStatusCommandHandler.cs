using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Commands.SetStockConfirmedOrderStatus;

// Regular CommandHandler
public class SetStockConfirmedOrderStatusCommandHandler(
    IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository)
        : IRequestHandler<SetStockConfirmedOrderStatusCommand, bool>
{
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> _orderRepository = orderRepository;

    /// <summary>
    /// Handler which processes the command when
    /// Stock service confirms the request
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<bool> Handle(SetStockConfirmedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        // Simulate a work time for confirming the stock
        await Task.Delay(10000, cancellationToken);

        Domain.AggregatesModel.OrderAggregate.Order? orderToUpdate =
            await this._orderRepository.SingleOrDefaultAsync(new GetOrderSpecification(command.ObjectId), cancellationToken);

        if (orderToUpdate is null)
        {
            return false;
        }

        orderToUpdate.SetStockConfirmedStatus();
        await this._orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
        return true;
    }
}


// Use for Idempotency in Command process
public class SetStockConfirmedOrderStatusIdentifiedCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<SetStockConfirmedOrderStatusCommand, bool>> logger) : IdentifiedCommandHandler<SetStockConfirmedOrderStatusCommand, bool>(mediator, requestManager, logger)
{
    protected override bool CreateResultForDuplicateRequest()
    {
        return true; // Ignore duplicate requests for processing order.
    }
}
