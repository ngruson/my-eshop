using Ardalis.Result;
using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Commands.SetPaidOrderStatus;

// Regular CommandHandler
public class SetPaidOrderStatusCommandHandler(IRepository<Order> orderRepository) : IRequestHandler<SetPaidOrderStatusCommand, Result>
{
    private readonly IRepository<Order> _orderRepository = orderRepository;

    /// <summary>
    /// Handler which processes the command when
    /// Shipment service confirms the payment
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result> Handle(SetPaidOrderStatusCommand command, CancellationToken cancellationToken)
    {
        // Simulate a work time for validating the payment
        await Task.Delay(10000, cancellationToken);

        Order? orderToUpdate = await this._orderRepository.SingleOrDefaultAsync(
            new GetOrderSpecification(command.OrderId), cancellationToken);

        if (orderToUpdate is null)
        {
            return Result.NotFound();
        }

        orderToUpdate.SetPaidStatus();
        await this._orderRepository.UpdateAsync(orderToUpdate, cancellationToken);

        return Result.Success();
    }
}


// Use for Idempotency in Command process
public class SetPaidIdentifiedOrderStatusCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<SetPaidOrderStatusCommand, Result>> logger)
        : IdentifiedCommandHandler<SetPaidOrderStatusCommand, Result>(mediator, requestManager, logger)
{
    protected override Result CreateResultForDuplicateRequest()
    {
        return Result.Success(); // Ignore duplicate requests for processing order.
    }
}
