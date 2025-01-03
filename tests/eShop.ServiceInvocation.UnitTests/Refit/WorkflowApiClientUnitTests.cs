using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Workflow.Contracts;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Refit;

public class WorkflowApiClientUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task confirm_grace_period(
            [Substitute, Frozen] IWorkflowApi workflowApi,
            WorkflowApiClient.Refit.WorkflowApiClient sut,
            string workflowInstanceId)
    {
        // Arrange

        // Act

        await sut.ConfirmGracePeriod(workflowInstanceId);

        // Assert

        await workflowApi.Received().ConfirmGracePeriod(workflowInstanceId);
    }

    [Theory, AutoNSubstituteData]
    public async Task create_order(
        [Substitute, Frozen] IWorkflowApi workflowApi,
        WorkflowApiClient.Refit.WorkflowApiClient sut,
        OrderDto order)
    {
        // Arrange

        // Act

        await sut.CreateOrder(order);

        // Assert

        await workflowApi.Received().CreateOrder(order);
    }
}
