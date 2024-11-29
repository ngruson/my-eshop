using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Ordering.API.Application.GuardClauses;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Contracts.GetOrder;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Queries.GetOrder;

internal class GetOrderQueryHandler(
    ILogger<GetOrderQueryHandler> logger,
    IRepository<Order> orderRepository)
    : IRequestHandler<GetOrderQuery, Result<OrderDto>>
{
    private readonly ILogger<GetOrderQueryHandler> logger = logger;
    private readonly IRepository<Order> orderRepository = orderRepository;

    public async Task<Result<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving order {ObjectId}", request.ObjectId);

            Order? order =
                await this.orderRepository.SingleOrDefaultAsync(new GetOrderSpecification(request.ObjectId), cancellationToken);

            Result foundResult = Guard.Against.OrderNull(order, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Order retrieved");

            return order!.Map();
        }
        catch (Exception ex)
        {
            string errorMessage = $"Failed to retrieve order {request.ObjectId}.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
