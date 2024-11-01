using Ardalis.Result;
using eShop.ServiceInvocation.OrderingApiClient;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Order.CreateOrder;

internal class CreateOrderCommandHandler(
    ILogger<CreateOrderCommandHandler> logger,
    IOrderingApiClient orderingApiClient) : IRequestHandler<CreateOrderCommand, Result>
{
    private readonly ILogger<CreateOrderCommandHandler> logger = logger;
    private readonly IOrderingApiClient orderingApiClient = orderingApiClient;

    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Creating order...");

            await this.orderingApiClient.CreateOrder(request.RequestId, request.Dto);

            this.logger.LogInformation("Order created");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to create order";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
