using Ardalis.Result;
using eShop.ServiceInvocation.OrderingApiClient;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Order.DeleteOrder;

internal class DeleteOrderCommandHandler(
    ILogger<DeleteOrderCommandHandler> logger,
    IOrderingApiClient orderingApiClient) : IRequestHandler<DeleteOrderCommand, Result>
{
    private readonly ILogger<DeleteOrderCommandHandler> logger = logger;
    private readonly IOrderingApiClient orderingApiClient = orderingApiClient;

    public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Deleting order {ObjectId}...", request.ObjectId);

            await this.orderingApiClient.DeleteOrder(request.ObjectId);

            this.logger.LogInformation("Order deleted");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to delete order";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
