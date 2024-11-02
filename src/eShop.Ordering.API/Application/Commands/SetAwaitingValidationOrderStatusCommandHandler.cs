using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Commands;

// Regular CommandHandler
public class SetAwaitingValidationOrderStatusCommandHandler(
    IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository) 
        : IRequestHandler<SetAwaitingValidationOrderStatusCommand, bool>
{
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> _orderRepository = orderRepository;

    /// <summary>
    /// Handler which processes the command when
    /// grace period has finished
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<bool> Handle(SetAwaitingValidationOrderStatusCommand command, CancellationToken cancellationToken)
    {
        Domain.AggregatesModel.OrderAggregate.Order? orderToUpdate = await this._orderRepository.SingleOrDefaultAsync(
            new GetOrderSpecification(command.OrderNumber),
            cancellationToken);

        if (orderToUpdate is null)
        {
            return false;
        }

        orderToUpdate.SetAwaitingValidationStatus();

        await this._orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
        return true;
    }
}


// Use for Idempotency in Command process
public class SetAwaitingValidationIdentifiedOrderStatusCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<SetAwaitingValidationOrderStatusCommand, bool>> logger) 
        : IdentifiedCommandHandler<SetAwaitingValidationOrderStatusCommand, bool>(mediator, requestManager, logger)
{
    protected override bool CreateResultForDuplicateRequest()
    {
        return true; // Ignore duplicate requests for processing order.
    }
}
