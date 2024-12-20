using Ardalis.Result;
using eShop.Ordering.Contracts.GetOrders;
using eShop.ServiceInvocation.OrderingApiClient;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Order.GetOrders;

internal class GetOrdersQueryHandler(
    ILogger<GetOrdersQueryHandler> logger,
    IOrderingApiClient orderingApiClient) : IRequestHandler<GetOrdersQuery, Result<List<OrderViewModel>>>
{
    private readonly ILogger<GetOrdersQueryHandler> logger = logger;
    private readonly IOrderingApiClient orderingApiClient = orderingApiClient;

    public async Task<Result<List<OrderViewModel>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            OrderDto[] orders = await this.orderingApiClient.GetOrders();

            return orders
                .ToList()
                .MapToOrderViewModelList();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve orders.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
