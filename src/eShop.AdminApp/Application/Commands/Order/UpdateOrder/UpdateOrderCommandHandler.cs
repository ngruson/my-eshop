using Ardalis.Result;
using eShop.ServiceInvocation.OrderingApiClient;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Order.UpdateOrder;

internal class UpdateOrderCommandHandler(
    ILogger<UpdateOrderCommandHandler> logger,
    IOrderingApiClient orderingApiClient) : IRequestHandler<UpdateOrderCommand, Result>
{
    private readonly ILogger<UpdateOrderCommandHandler> logger = logger;
    private readonly IOrderingApiClient orderingApiClient = orderingApiClient;

    public async Task<Result> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Updating order {ObjectId}...", request.ObjectId);

            await this.orderingApiClient.UpdateOrder(request.ObjectId, request.Dto);

            this.logger.LogInformation("Order updated");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to update order";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
