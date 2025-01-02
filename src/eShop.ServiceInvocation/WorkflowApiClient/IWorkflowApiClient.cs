namespace eShop.ServiceInvocation.WorkflowApiClient;

public interface IWorkflowApiClient
{    
    Task ConfirmGracePeriod(string workflowInstanceId);
    Task CreateOrder(Ordering.Contracts.CreateOrder.OrderDto dto);
}
