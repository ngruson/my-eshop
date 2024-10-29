using Ardalis.Result;
using eShop.ServiceInvocation.OrderingService;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Order.CreateOrder;

internal class CreateOrderCommandHandler(
    ILogger<CreateOrderCommandHandler> logger,
    IOrderingService orderingService) : IRequestHandler<CreateOrderCommand, Result>
{
    private readonly ILogger<CreateOrderCommandHandler> logger = logger;
    private readonly IOrderingService orderingService = orderingService;

    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Creating order...");

            await this.orderingService.CreateOrder(request.RequestId, request.Dto);

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
