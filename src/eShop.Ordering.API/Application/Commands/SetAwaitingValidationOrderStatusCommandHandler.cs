using Ardalis.Result;
using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Commands;

// Regular CommandHandler
public class SetAwaitingValidationOrderStatusCommandHandler(
    IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository) 
        : IRequestHandler<SetAwaitingValidationOrderStatusCommand, Result>
{
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> _orderRepository = orderRepository;

    /// <summary>
    /// Handler which processes the command when
    /// grace period has finished
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result> Handle(SetAwaitingValidationOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var orderToUpdate = await this._orderRepository.SingleOrDefaultAsync(
            new GetOrderSpecification(command.OrderNumber),
            cancellationToken);

        if (orderToUpdate is null)
        {
            return Result.NotFound();
        }

        orderToUpdate.SetAwaitingValidationStatus();

        await this._orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
        return Result.Success();
    }
}


// Use for Idempotency in Command process
public class SetAwaitingValidationIdentifiedOrderStatusCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<SetAwaitingValidationOrderStatusCommand, Result>> logger) 
        : IdentifiedCommandHandler<SetAwaitingValidationOrderStatusCommand, Result>(mediator, requestManager, logger)
{
    protected override Result CreateResultForDuplicateRequest()
    {
        return Result.Success(); // Ignore duplicate requests for processing order.
    }
}
