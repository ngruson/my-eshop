using Ardalis.Result;
using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Commands.CancelOrder;

// Regular CommandHandler
public class CancelOrderCommandHandler(IRepository<Order> orderRepository) : IRequestHandler<CancelOrderCommand, Result>
{
    private readonly IRepository<Order> _orderRepository = orderRepository;

    /// <summary>
    /// Handler which processes the command when
    /// customer executes cancel order from app
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        Order? orderToUpdate =
            await this._orderRepository.SingleOrDefaultAsync(new GetOrderSpecification(command.ObjectId), cancellationToken);

        if (orderToUpdate is null)
        {
            return Result.NotFound();
        }

        orderToUpdate.SetCancelledStatus();
        await this._orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
        return Result.Success();
    }
}

// Use for Idempotency in Command process
public class CancelOrderIdentifiedCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<CancelOrderCommand, Result>> logger) : IdentifiedCommandHandler<CancelOrderCommand, Result>(mediator, requestManager, logger)
{
    protected override Result CreateResultForDuplicateRequest()
    {
        return Result.Success(); // Ignore duplicate requests for processing order.
    }
}
