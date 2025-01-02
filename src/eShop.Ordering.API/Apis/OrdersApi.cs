using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using eShop.Ordering.API.Application.Commands.CancelOrder;
using eShop.Ordering.API.Application.Commands.CreateOrder;
using eShop.Ordering.API.Application.Commands.CreateOrderDraft;
using eShop.Ordering.API.Application.Commands.SetAwaitingValidationOrderStatus;
using eShop.Ordering.API.Application.Commands.SetPaidOrderStatus;
using eShop.Ordering.API.Application.Commands.SetStockConfirmedOrderStatus;
using eShop.Ordering.API.Application.Commands.SetStockRejectedOrderStatus;
using eShop.Ordering.API.Application.Commands.ShipOrder;
using eShop.Ordering.API.Application.Commands.UpdateOrder;
using eShop.Ordering.API.Application.Queries.GetCardTypes;
using eShop.Ordering.API.Application.Queries.GetOrder;
using eShop.Ordering.API.Application.Queries.GetOrders;
using eShop.Ordering.API.Application.Queries.GetOrdersFromUser;

namespace eShop.Ordering.API.Apis;

public static class OrdersApi
{
    public static RouteGroupBuilder MapOrdersApiV1(this IEndpointRouteBuilder app, bool workflowsEnabled)
    {
        RouteGroupBuilder api = app.MapGroup("api/orders").HasApiVersion(1.0);

        api.MapPut("/cancel", async (
            CancelOrderCommand command,
            [FromServices] IMediator mediator,
            [FromHeader(Name = "x-requestid")] Guid requestId) =>
                (await mediator.Send(new IdentifiedCommand<CancelOrderCommand, Result>(command, requestId)))
                    .ToMinimalApiResult());

        api.MapPut("/ship", async (
            ShipOrderCommand command,
            [FromServices] IMediator mediator,
            [FromHeader(Name = "x-requestid")] Guid requestId) =>
                (await mediator.Send(new IdentifiedCommand<ShipOrderCommand, Result>(command, requestId)))
                    .ToMinimalApiResult());

        api.MapGet("/all", async ([FromServices] IMediator mediator) =>
            (await mediator.Send(new GetOrdersQuery()))
                .ToMinimalApiResult());

        api.MapGet("/{objectId}", async (Guid objectId, [FromServices] IMediator mediator) =>
            (await mediator.Send(new GetOrderQuery(objectId)))
                .ToMinimalApiResult());

        api.MapGet("/", async ([FromServices] IMediator mediator) =>
            (await mediator.Send(new GetOrdersFromUserQuery()))
                .ToMinimalApiResult());

        api.MapGet("/cardTypes", async ([FromServices] IMediator mediator) =>
            (await mediator.Send(new GetCardTypesQuery()))
                .ToMinimalApiResult());

        api.MapPost("/draft", async ([FromBody] CreateOrderDraftCommand command, [FromServices] IMediator mediator) =>
            (await mediator.Send(command))
                .ToMinimalApiResult());

        api.MapPost("/", async (
            CreateOrderCommand command,
            [FromServices] IMediator mediator,
            [FromHeader(Name = "x-requestid")] Guid requestId) =>
                (await mediator.Send(new IdentifiedCommand<CreateOrderCommand, Result<Guid>>(command, requestId)))
                    .ToMinimalApiResult());

        api.MapPut("/{objectId}", async (Guid objectId, [FromBody] Contracts.UpdateOrder.OrderDto dto, [FromServices] IMediator mediator) =>
            (await mediator.Send(new UpdateOrderCommand(objectId, dto)))
                .ToMinimalApiResult());

        if (workflowsEnabled)
        {
            api.MapPost("/confirmGracePeriod/{objectId}", async(Guid objectId, [FromServices] IMediator mediator) =>
                (await mediator.Send(new SetAwaitingValidationOrderStatusCommand(objectId)))
                    .ToMinimalApiResult());

            api.MapPost("/confirmStock/{objectId}", async (Guid objectId, [FromServices] IMediator mediator) =>
                (await mediator.Send(new SetStockConfirmedOrderStatusCommand(objectId)))
                    .ToMinimalApiResult());

            api.MapPost("/rejectStock/{objectId}", async (Guid objectId, Guid[] orderStockItems, [FromServices] IMediator mediator) =>
                (await mediator.Send(new SetStockRejectedOrderStatusCommand(objectId, orderStockItems)))
                    .ToMinimalApiResult());

            api.MapPost("/paid/{objectId}", async (Guid objectId, [FromServices] IMediator mediator) =>
                (await mediator.Send(new SetPaidOrderStatusCommand(objectId)))
                    .ToMinimalApiResult());
        }

        return api;
    }
}
