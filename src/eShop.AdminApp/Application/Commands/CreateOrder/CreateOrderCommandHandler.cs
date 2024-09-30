using Ardalis.Result;
using eShop.Ordering.Contracts;
using MediatR;

namespace eShop.AdminApp.Application.Commands.CreateOrder;

internal class CreateOrderCommandHandler(
    ILogger<CreateOrderCommandHandler> logger,
    IOrderingApi orderingApi) : IRequestHandler<CreateOrderCommand, Result>
{
    private readonly ILogger<CreateOrderCommandHandler> logger = logger;
    private readonly IOrderingApi orderingApi = orderingApi;

    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Creating order...");

            await this.orderingApi.CreateOrder(request.RequestId, request.Dto);

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
