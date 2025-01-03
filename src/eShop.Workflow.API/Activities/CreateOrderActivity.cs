using Ardalis.Result;
using Dapr.Workflow;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.ServiceInvocation.OrderingApiClient;
using eShop.ServiceInvocation.WebAppApiClient;

namespace eShop.Workflow.API.Activities;

internal class CreateOrderActivity(ILogger<CreateOrderActivity> logger, IOrderingApiClient orderingApiClient, IWebAppApiClient webAppApiClient) : WorkflowActivity<OrderDto, Result<Guid>>
{
    public override async Task<Result<Guid>> RunAsync(WorkflowActivityContext context, OrderDto input)
    {
        try
        {
            logger.LogInformation("Creating order");
            Guid orderId = await orderingApiClient.CreateOrder(Guid.Parse(context.InstanceId), input with { WorkflowInstanceId = context.InstanceId });
            logger.LogInformation("Order created: {OrderId}", orderId);

            logger.LogInformation("Notifying web app of order status change");
            await webAppApiClient.NotifyOrderStatusChange(input.UserId.ToString());
            
            return orderId;
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to create order.";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
