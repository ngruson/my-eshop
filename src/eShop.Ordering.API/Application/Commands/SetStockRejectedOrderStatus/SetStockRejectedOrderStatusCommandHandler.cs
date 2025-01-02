using Ardalis.Result;
using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Commands.SetStockRejectedOrderStatus;

// Regular CommandHandler
public class SetStockRejectedOrderStatusCommandHandler(IRepository<Order> orderRepository) : IRequestHandler<SetStockRejectedOrderStatusCommand, Result>
{
    /// <summary>
    /// Handler which processes the command when
    /// Stock service rejects the request
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result> Handle(SetStockRejectedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        // Simulate a work time for rejecting the stock
        await Task.Delay(10000, cancellationToken);

        Order? orderToUpdate = await orderRepository.SingleOrDefaultAsync(new GetOrderSpecification(command.ObjectId), cancellationToken);

        if (orderToUpdate is null)
        {
            return Result.NotFound();
        }

        orderToUpdate.SetCancelledStatusWhenStockIsRejected(command.OrderStockItems);

        await orderRepository.UpdateAsync(orderToUpdate, cancellationToken);

        return Result.Success();
    }
}

// Use for Idempotency in Command process
public class SetStockRejectedOrderStatusIdentifiedCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<SetStockRejectedOrderStatusCommand, Result>> logger) : IdentifiedCommandHandler<SetStockRejectedOrderStatusCommand, Result>(mediator, requestManager, logger)
{
    protected override Result CreateResultForDuplicateRequest()
    {
        return Result.Success(); // Ignore duplicate requests for processing order.
    }
}
