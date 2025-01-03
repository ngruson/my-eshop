using Dapr.Client;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.ServiceInvocation.Auth;

namespace eShop.ServiceInvocation.WorkflowApiClient.Dapr;

public class WorkflowApiClient(DaprClient daprClient, AccessTokenAccessorFactory accessTokenAccessorFactory)
    : BaseDaprApiClient(daprClient, accessTokenAccessorFactory), IWorkflowApiClient
{
    private readonly string basePath = "/api/orders";
    protected override string AppId => "workflow-api";

    public async Task ConfirmGracePeriod(string workflowInstanceId)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            $"{this.basePath}/confirmGracePeriod/{workflowInstanceId}");

        await this.DaprClient.InvokeMethodAsync(request);
    }

    public async Task CreateOrder(OrderDto dto)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            this.basePath,
            null,
            dto);        

        await this.DaprClient.InvokeMethodAsync(request);
    }
}
