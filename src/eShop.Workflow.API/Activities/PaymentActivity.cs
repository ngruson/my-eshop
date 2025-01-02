using Ardalis.Result;
using Dapr.Workflow;
using eShop.PaymentProcessor.Contracts;
using eShop.ServiceInvocation.OrderingApiClient;
using eShop.ServiceInvocation.PaymentProcessorApiClient;
using eShop.ServiceInvocation.WebAppApiClient;

namespace eShop.Workflow.API.Activities;

public class PaymentActivity(ILogger<PaymentActivity> logger, IOrderingApiClient orderingApiClient, IPaymentProcessorApiClient paymentProcessorApiClient, IWebAppApiClient webAppApiClient)
    : WorkflowActivity<PaymentActivityInput, Result>
{
    public override async Task<Result> RunAsync(WorkflowActivityContext context, PaymentActivityInput input)
    {
        try
        {
            logger.LogInformation("Processing payment for order {OrderId}", input.OrderId);
            PaymentStatus paymentStatus = await paymentProcessorApiClient.ProcessPayment();

            logger.LogInformation("Notifying web app of order status change");
            await webAppApiClient.NotifyOrderStatusChange(input.BuyerId.ToString());

            if (paymentStatus == PaymentStatus.Succeeded)
            {
                logger.LogInformation("Payment succeeded for order {OrderId}", input.OrderId);
                await orderingApiClient.Paid(input.OrderId);
            }
            else
            {
                logger.LogInformation("Payment failed for order {OrderId}, cancelling order", input.OrderId);
                await orderingApiClient.Cancel(input.OrderId);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = $"Failed to process payment for order {input.OrderId}";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
