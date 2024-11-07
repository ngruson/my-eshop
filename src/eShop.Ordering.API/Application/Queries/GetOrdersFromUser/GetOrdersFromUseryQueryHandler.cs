using Ardalis.Result;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Contracts.GetOrdersFromUser;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Queries.GetOrdersFromUser;

internal class GetOrdersFromUserQueryHandler(
    ILogger<GetOrdersFromUserQueryHandler> logger,
    IRepository<Order> orderRepository,
    IIdentityService identityService)
        : IRequestHandler<GetOrdersFromUserQuery, Result<OrderDto[]>>
{
    private readonly ILogger<GetOrdersFromUserQueryHandler> logger = logger;
    private readonly IRepository<Order> orderRepository = orderRepository;
    private readonly IIdentityService identityService = identityService;

    public async Task<Result<OrderDto[]>> Handle(GetOrdersFromUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Guid? userId = this.identityService.GetUserIdentity();
            this.logger.LogInformation("Retrieving orders for user {User}", userId);

            List<Order> orders =
                await this.orderRepository.ListAsync(new GetOrdersFromUserSpecification(userId!.Value), cancellationToken);

            this.logger.LogInformation("Orders retrieved: {Count}", orders.Count);

            return orders.Map();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve orders.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
