using Ardalis.Result;
using Dapr.Workflow;
using eShop.ServiceInvocation.OrderingApiClient;

namespace eShop.Workflow.API.Activities;

internal class ConfirmGracePeriodActivity(ILogger<CreateOrderActivity> logger,    
    IOrderingApiClient orderingApiClient) : WorkflowActivity<Guid, Result>
{
    public override async Task<Result> RunAsync(WorkflowActivityContext context, Guid input)
    {
        try
        {
            logger.LogInformation("Confirming grace period for order {OrderId}", input);
            await orderingApiClient.ConfirmGracePeriod(input);

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = $"Failed to confirm grace period for order {input}";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
