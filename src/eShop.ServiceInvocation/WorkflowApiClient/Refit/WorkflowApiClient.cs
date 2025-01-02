using eShop.Ordering.Contracts.CreateOrder;
using eShop.Workflow.Contracts;

namespace eShop.ServiceInvocation.WorkflowApiClient.Refit;

public class WorkflowApiClient(IWorkflowApi workflowApi) : IWorkflowApiClient
{
    public async Task ConfirmGracePeriod(string workflowInstanceId)
    {
        await workflowApi.ConfirmGracePeriod(workflowInstanceId);
    }

    public async Task CreateOrder(OrderDto dto)
    {
        await workflowApi.CreateOrder(dto);
    }
}
