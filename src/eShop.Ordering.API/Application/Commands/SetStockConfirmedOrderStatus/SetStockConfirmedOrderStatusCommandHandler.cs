using Ardalis.Result;
using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Commands.SetStockConfirmedOrderStatus;

// Regular CommandHandler
public class SetStockConfirmedOrderStatusCommandHandler(IRepository<Order> orderRepository) : IRequestHandler<SetStockConfirmedOrderStatusCommand, Result>
{
    private readonly IRepository<Order> _orderRepository = orderRepository;

    /// <summary>
    /// Handler which processes the command when
    /// Stock service confirms the request
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result> Handle(SetStockConfirmedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        // Simulate a work time for confirming the stock
        await Task.Delay(10000, cancellationToken);

        Order? orderToUpdate =
            await this._orderRepository.SingleOrDefaultAsync(new GetOrderSpecification(command.ObjectId), cancellationToken);

        if (orderToUpdate is null)
        {
            return Result.NotFound();
        }

        orderToUpdate.SetStockConfirmedStatus();
        await this._orderRepository.UpdateAsync(orderToUpdate, cancellationToken);

        return Result.Success();
    }
}


// Use for Idempotency in Command process
public class SetStockConfirmedOrderStatusIdentifiedCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<SetStockConfirmedOrderStatusCommand, Result>> logger) : IdentifiedCommandHandler<SetStockConfirmedOrderStatusCommand, Result>(mediator, requestManager, logger)
{
    protected override Result CreateResultForDuplicateRequest()
    {
        return Result.Success(); // Ignore duplicate requests for processing order.
    }
}
