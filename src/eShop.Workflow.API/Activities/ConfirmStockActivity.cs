using Ardalis.Result;
using Dapr.Workflow;
using eShop.ServiceInvocation.OrderingApiClient;
using eShop.ServiceInvocation.WebAppApiClient;

namespace eShop.Workflow.API.Activities;

public class ConfirmStockActivity(ILogger<ConfirmStockActivity> logger, IOrderingApiClient orderingApiClient, IWebAppApiClient webAppApiClient)
    : WorkflowActivity<ConfirmStockActivityInput, Result>
{
    public override async Task<Result> RunAsync(WorkflowActivityContext context, ConfirmStockActivityInput input)
    {
        try
        {
            logger.LogInformation("Confirming stock for order {OrderId}", input.OrderId);

            if (input.ConfirmedOrderStockItems.Any(_ => !_.HasStock))
            {
                await orderingApiClient.RejectStock(input.OrderId, [.. input.ConfirmedOrderStockItems.Select(_ => _.ProductId)]);
                logger.LogInformation("Stock rejected for order {OrderId}", input.OrderId);

                await webAppApiClient.NotifyOrderStatusChange(input.UserId.ToString());

                return Result.Conflict();
            }
            else
            {
                await orderingApiClient.ConfirmStock(input.OrderId);
                logger.LogInformation("Stock confirmed for order {OrderId}", input.OrderId);

                await webAppApiClient.NotifyOrderStatusChange(input.UserId.ToString());

                return Result.Success();
            }            
        }
        catch (Exception ex)
        {
            string errorMessage = $"Failed to confirm/reject stock for order {input.OrderId}";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
