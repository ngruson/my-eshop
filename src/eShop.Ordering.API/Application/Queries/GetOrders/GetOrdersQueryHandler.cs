using Ardalis.Result;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Contracts.GetOrders;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Queries.GetOrders;

internal class GetOrdersQueryHandler(
    ILogger<GetOrdersQueryHandler> logger,
    IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository)
        : IRequestHandler<GetOrdersQuery, Result<List<OrderDto>>>
{
    private readonly ILogger<GetOrdersQueryHandler> logger = logger;
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository = orderRepository;

    public async Task<Result<List<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving orders");

            List<Domain.AggregatesModel.OrderAggregate.Order> orders =
                await this.orderRepository.ListAsync(new GetOrdersSpecification(), cancellationToken);

            this.logger.LogInformation("Orders retrieved: {Count}", orders.Count);

            return orders
                .MapToOrderDtoList();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve orders.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
