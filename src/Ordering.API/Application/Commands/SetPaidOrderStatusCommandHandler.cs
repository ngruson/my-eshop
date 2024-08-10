using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Commands;

// Regular CommandHandler
public class SetPaidOrderStatusCommandHandler(IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository) 
    : IRequestHandler<SetPaidOrderStatusCommand, bool>
{
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> _orderRepository = orderRepository;

    /// <summary>
    /// Handler which processes the command when
    /// Shipment service confirms the payment
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<bool> Handle(SetPaidOrderStatusCommand command, CancellationToken cancellationToken)
    {
        // Simulate a work time for validating the payment
        await Task.Delay(10000, cancellationToken);

        var orderToUpdate = await this._orderRepository.SingleOrDefaultAsync(
            new GetOrderSpecification(command.OrderNumber),
            cancellationToken);

        if (orderToUpdate is null)
        {
            return false;
        }

        orderToUpdate.SetPaidStatus();
        return await this._orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}


// Use for Idempotency in Command process
public class SetPaidIdentifiedOrderStatusCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<SetPaidOrderStatusCommand, bool>> logger) 
        : IdentifiedCommandHandler<SetPaidOrderStatusCommand, bool>(mediator, requestManager, logger)
{
    protected override bool CreateResultForDuplicateRequest()
    {
        return true; // Ignore duplicate requests for processing order.
    }
}
