using eShop.Ordering.Contracts.CreateOrder;
using Refit;

namespace eShop.Workflow.Contracts;

public interface IWorkflowApi
{
    [Post("/api/orders?api-version=1.0")]
    Task CreateOrder(OrderDto dto);

    [Post("/api/orders/confirmGracePeriod?api-version=1.0")]
    Task ConfirmGracePeriod(string workflowInstanceId);
}
