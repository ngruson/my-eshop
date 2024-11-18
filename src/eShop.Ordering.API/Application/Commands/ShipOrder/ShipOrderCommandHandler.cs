using Ardalis.Result;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.Exceptions;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Commands.ShipOrder;

// Regular CommandHandler
public class ShipOrderCommandHandler(IRepository<Order> orderRepository) : IRequestHandler<ShipOrderCommand, Result>
{
    private readonly IRepository<Order> _orderRepository = orderRepository;

    /// <summary>
    /// Handler which processes the command when
    /// administrator executes ship order from app
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result> Handle(ShipOrderCommand command, CancellationToken cancellationToken)
    {
        try
        {
            Order? orderToUpdate =
            await this._orderRepository.SingleOrDefaultAsync(new GetOrderSpecification(command.ObjectId), cancellationToken);

            if (orderToUpdate is null)
            {
                return Result.NotFound();
            }

            orderToUpdate.SetShippedStatus();
            await this._orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
            return Result.Success();
        }
        catch (OrderingDomainException)
        {
            return Result.Conflict();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}

// Use for Idempotency in Command process
public class ShipOrderIdentifiedCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<ShipOrderCommand, Result>> logger) : IdentifiedCommandHandler<ShipOrderCommand, Result>(mediator, requestManager, logger)
{
    protected override Result CreateResultForDuplicateRequest()
    {
        return Result.Success(); // Ignore duplicate requests for processing order.
    }
}
