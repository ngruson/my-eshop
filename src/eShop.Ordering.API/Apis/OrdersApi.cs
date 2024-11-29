using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using eShop.Ordering.API.Application.Commands.CancelOrder;
using eShop.Ordering.API.Application.Commands.CreateOrder;
using eShop.Ordering.API.Application.Commands.CreateOrderDraft;
using eShop.Ordering.API.Application.Commands.ShipOrder;
using eShop.Ordering.API.Application.Commands.UpdateOrder;
using eShop.Ordering.API.Application.Queries.GetCardTypes;
using eShop.Ordering.API.Application.Queries.GetOrder;
using eShop.Ordering.API.Application.Queries.GetOrders;
using eShop.Ordering.API.Application.Queries.GetOrdersFromUser;

namespace eShop.Ordering.API.Apis;

public static class OrdersApi
{
    public static RouteGroupBuilder MapOrdersApiV1(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.MapGroup("api/orders").HasApiVersion(1.0);

        api.MapPut("/cancel/{objectId}", async (
            CancelOrderCommand command,
            [FromServices] IMediator mediator,
            [FromHeader(Name = "x-requestid")] Guid requestId) =>
                (await mediator.Send(new IdentifiedCommand<CancelOrderCommand, Result>(command, requestId)))
                    .ToMinimalApiResult());

        api.MapPut("/ship/{objectId}", async (
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
                (await mediator.Send(new IdentifiedCommand<CreateOrderCommand, Result>(command, requestId)))
                    .ToMinimalApiResult());

        api.MapPut("/{objectId}", async (Guid objectId, [FromBody] Contracts.UpdateOrder.OrderDto dto, [FromServices] IMediator mediator) =>
            (await mediator.Send(new UpdateOrderCommand(objectId, dto)))
                .ToMinimalApiResult());

        return api;
    }
}
