using Ardalis.Result;
using Dapr.Workflow;
using eShop.Catalog.Contracts.AssessStockItemsForOrder;
using eShop.ServiceInvocation.CatalogApiClient;
using eShop.ServiceInvocation.WebAppApiClient;

namespace eShop.Workflow.API.Activities;

internal class AssessStockItemsActivity(ILogger<AssessStockItemsActivity> logger, ICatalogApiClient catalogApiClient, IWebAppApiClient webAppApiClient)
    : WorkflowActivity<AssessStockItemsActivityInput, Result<AssessStockItemsForOrderResponseDto>>
{
    public override async Task<Result<AssessStockItemsForOrderResponseDto>> RunAsync(WorkflowActivityContext context, AssessStockItemsActivityInput input)
    {
        try
        {
            AssessStockItemsForOrderResponseDto dto = await catalogApiClient.AssessStockItemsForOrder(
                new AssessStockItemsForOrderRequestDto(input.OrderId, input.OrderStockItems));

            await webAppApiClient.NotifyOrderStatusChange(input.UserId.ToString());

            return dto;
        }
        catch (Exception ex)
        {
            string errorMessage = $"Failed to confirm grace period for order {input}";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
