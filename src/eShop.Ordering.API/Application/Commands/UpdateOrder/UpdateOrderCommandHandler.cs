using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Ordering.API.Application.GuardClauses;
using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Commands.UpdateOrder;

internal class UpdateOrderCommandHandler(
    ILogger<UpdateOrderCommandHandler> logger,
    IRepository<Domain.AggregatesModel.OrderAggregate.Order> repository) : IRequestHandler<UpdateOrderCommand, Result>
{
    private readonly ILogger<UpdateOrderCommandHandler> logger = logger;
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> repository = repository;

    public async Task<Result> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Updating order...");

            Domain.AggregatesModel.OrderAggregate.Order? order =
                await this.repository.SingleOrDefaultAsync(
                    new GetOrderSpecification(request.ObjectId),
                    cancellationToken);

            Result foundResult = Guard.Against.OrderNull(order, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            request.Dto.Map(order!);

            await this.repository.UpdateAsync(order!, cancellationToken);

            this.logger.LogInformation("Order updated");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to update order.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
