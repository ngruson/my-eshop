using Ardalis.Result;
using eShop.Ordering.Contracts.GetOrder;
using eShop.ServiceInvocation.OrderingApiClient;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Order.GetOrder;

internal class GetOrderQueryHandler(
    ILogger<GetOrderQueryHandler> logger,
    IOrderingApiClient orderingApiClient) : IRequestHandler<GetOrderQuery, Result<OrderViewModel>>
{
    public async Task<Result<OrderViewModel>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Retrieving order {ObjectId} from API...", request.ObjectId);

            OrderDto order = await orderingApiClient.GetOrder(request.ObjectId);

            logger.LogInformation("Retrieved order {ObjectId} from API", request.ObjectId);

            return order.Map();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve order.";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
