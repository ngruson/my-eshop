using Dapr.Workflow;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Workflow.API.Workflows;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Workflow.API;

internal static class RoutingExtensions
{
    public static RouteGroupBuilder MapOrdersApiV1(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.MapGroup("api/orders").HasApiVersion(1.0);

        api.MapPost("/", async (DaprWorkflowClient client, [FromBody] OrderDto order) =>
        {
            await client.ScheduleNewWorkflowAsync(nameof(OrderProcessingWorkflow), null, order);
        });

        api.MapPost("/confirmGracePeriod/{instanceId}", async (DaprWorkflowClient client, [FromRoute] string instanceId) =>
        {
            await client.RaiseEventAsync(instanceId, EventNames.ConfirmGracePeriod);
        });

        api.MapGet("/{workflowInstanceId}", async ([FromRoute] string workflowInstanceId, DaprWorkflowClient daprClient) =>
        {
            WorkflowState workflowState = await daprClient.GetWorkflowStateAsync(workflowInstanceId);
            return workflowState != null ? Results.Ok(workflowState) : Results.NotFound();
        });

        return api;
    }
}
