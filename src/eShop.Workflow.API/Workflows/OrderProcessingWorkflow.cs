using Ardalis.Result;
using Dapr.Workflow;
using eShop.Catalog.Contracts.AssessStockItemsForOrder;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Workflow.API.Activities;

namespace eShop.Workflow.API.Workflows;

internal class OrderProcessingWorkflow : Workflow<OrderDto, Result>
{
    public override async Task<Result> RunAsync(WorkflowContext context, OrderDto input)
    {
        try
        {
            //logger.LogInformation("Order processing started");

            Result<Guid> createOrderResult = await context.CallActivityAsync<Result<Guid>>(nameof(CreateOrderActivity), input);
            Guid orderId = createOrderResult.Value;

            //Wait for the order processor to confirm the grace period
            await context.WaitForExternalEventAsync<object>(EventNames.ConfirmGracePeriod);
            await context.CallActivityAsync(nameof(ConfirmGracePeriodActivity), orderId);

            Result<AssessStockItemsForOrderResponseDto> assessStockItemsResult =
                await context.CallActivityAsync<Result<AssessStockItemsForOrderResponseDto>>(nameof(AssessStockItemsActivity),
                    new AssessStockItemsActivityInput(orderId, input.UserId,
                        [.. input.Items.Select(_ => new OrderStockItem(_.ProductId, _.Units))]));

            if (assessStockItemsResult.IsSuccess)
            {
                Result confirmStockResult = await context.CallActivityAsync<Result>(nameof(ConfirmStockActivity),
                    new ConfirmStockActivityInput(orderId, input.UserId, assessStockItemsResult.Value.ConfirmedOrderStockItems));

                if (confirmStockResult.IsSuccess)
                {
                    Result paymentResult = await context.CallActivityAsync<Result>(nameof(PaymentActivity),
                        new PaymentActivityInput(orderId, input.UserId));
                }
            }

            //logger.LogInformation("Order processing completed");

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
