using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Dapr.Workflow;
using eShop.Catalog.Contracts.AssessStockItemsForOrder;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Workflow.API.Activities;
using eShop.Workflow.API.Workflows;
using NSubstitute;

namespace eShop.Workflow.UnitTests;

public class OrderProcessingWorkflowUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_success_when_workflow_succeeds(
        [Substitute, Frozen] WorkflowContext workflowContext,
        Guid orderId,
        OrderDto order,
        AssessStockItemsForOrderResponseDto assessStockItemsForOrderResponseDto,
        OrderProcessingWorkflow sut)
    {
        // Arrange

        workflowContext.CallActivityAsync<Result<Guid>>(nameof(CreateOrderActivity), order)
            .Returns(orderId);

        workflowContext.CallActivityAsync<Result<AssessStockItemsForOrderResponseDto>>(nameof(AssessStockItemsActivity), Arg.Any<AssessStockItemsActivityInput>())
            .Returns(assessStockItemsForOrderResponseDto);

        workflowContext.CallActivityAsync<Result>(nameof(ConfirmStockActivity), Arg.Any<ConfirmStockActivityInput>())
            .Returns(Result.Success());

        workflowContext.CallActivityAsync<Result>(nameof(PaymentActivity), Arg.Any<PaymentActivityInput>())
            .Returns(Result.Success());

        // Act

        Result result = await sut.RunAsync(workflowContext, order);

        // Assert

        Assert.True(result.IsSuccess);
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_error_when_workflow_fails(
        [Substitute, Frozen] WorkflowContext workflowContext,
        OrderDto order,
        OrderProcessingWorkflow sut)
    {
        // Arrange

        // Act

        Result result = await sut.RunAsync(workflowContext, order);

        // Assert

        Assert.True(result.IsError());
    }
}
