using Ardalis.Result;
using eShop.AdminApp.Services;
using eShop.Ordering.Contracts.GetOrders;
using MediatR;

namespace eShop.AdminApp.Application.Queries.GetOrders;

internal class GetOrdersQueryHandler(
    ILogger<GetOrdersQueryHandler> logger,
    OrderingService orderingService) : IRequestHandler<GetOrdersQuery, Result<List<OrderViewModel>>>
{
    private readonly ILogger<GetOrdersQueryHandler> logger = logger;
    private readonly OrderingService orderingService = orderingService;

    public async Task<Result<List<OrderViewModel>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            OrderDto[] orders = await this.orderingService.GetOrders();

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
