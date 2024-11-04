using Ardalis.Result.AspNetCore;
using eShop.Ordering.API.Application.Commands.CancelOrder;
using eShop.Ordering.API.Application.Commands.CreateOrder;
using eShop.Ordering.API.Application.Commands.CreateOrderDraft;
using eShop.Ordering.API.Application.Commands.ShipOrder;
using eShop.Ordering.API.Application.Commands.UpdateOrder;
using eShop.Ordering.API.Application.Queries.GetOrder;
using eShop.Ordering.API.Application.Queries.GetOrders;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Contracts.GetCardTypes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace eShop.Ordering.API.Apis;

public static class OrdersApi
{
    public static RouteGroupBuilder MapOrdersApiV1(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.MapGroup("api/orders").HasApiVersion(1.0);

        api.MapPut("/cancel", CancelOrderAsync);
        api.MapPut("/ship", ShipOrderAsync);

        api.MapGet("/all", async ([FromServices] IMediator mediator) =>
            (await mediator.Send(new GetOrdersQuery()))
                .ToMinimalApiResult());

        api.MapGet("/{objectId}", async (Guid objectId, [FromServices] IMediator mediator) =>
            (await mediator.Send(new GetOrderQuery(objectId)))
                .ToMinimalApiResult());

        api.MapGet("/", GetOrdersByUserAsync);
        api.MapGet("/cardTypes", GetCardTypesAsync);
        api.MapPost("/draft", CreateOrderDraftAsync);
        api.MapPost("/", CreateOrderAsync);

        api.MapPut("/{objectId}", async (Guid objectId, [FromBody] Contracts.UpdateOrder.OrderDto dto, [FromServices] IMediator mediator) =>
            (await mediator.Send(new UpdateOrderCommand(objectId, dto)))
                .ToMinimalApiResult());

        return api;
    }

    public static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> CancelOrderAsync(
        [FromHeader(Name = "x-requestid")] Guid requestId,
        CancelOrderCommand command,
        [AsParameters] OrderServices services)
    {
        if (requestId == Guid.Empty)
        {
            return TypedResults.BadRequest("Empty GUID is not valid for request ID");
        }

        IdentifiedCommand<CancelOrderCommand, bool> requestCancelOrder = new(command, requestId);

        services.Logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            requestCancelOrder.GetGenericTypeName(),
            nameof(requestCancelOrder.Command.OrderNumber),
            requestCancelOrder.Command.OrderNumber,
            requestCancelOrder);

        bool commandResult = await services.Mediator.Send(requestCancelOrder);

        if (!commandResult)
        {
            return TypedResults.Problem(detail: "Cancel order failed to process.", statusCode: 500);
        }

        return TypedResults.Ok();
    }

    public static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> ShipOrderAsync(
        [FromHeader(Name = "x-requestid")] Guid requestId,
        ShipOrderCommand command,
        [AsParameters] OrderServices services)
    {
        if (requestId == Guid.Empty)
        {
            return TypedResults.BadRequest("Empty GUID is not valid for request ID");
        }

        IdentifiedCommand<ShipOrderCommand, bool> requestShipOrder = new(command, requestId);

        services.Logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            requestShipOrder.GetGenericTypeName(),
            nameof(requestShipOrder.Command.OrderNumber),
            requestShipOrder.Command.OrderNumber,
            requestShipOrder);

        bool commandResult = await services.Mediator.Send(requestShipOrder);

        if (!commandResult)
        {
            return TypedResults.Problem(detail: "Ship order failed to process.", statusCode: 500);
        }

        return TypedResults.Ok();
    }

    public static async Task<Ok<IEnumerable<OrderSummary>>> GetOrdersByUserAsync([AsParameters] OrderServices services)
    {
        string? userId = services.IdentityService.GetUserIdentity();
        IEnumerable<OrderSummary> orders = await services.Queries.GetOrdersFromUserAsync(userId!);
        return TypedResults.Ok(orders);
    }

    public static async Task<Ok<IEnumerable<CardTypeDto>>> GetCardTypesAsync(IOrderQueries orderQueries)
    {
        IEnumerable<CardTypeDto> cardTypes = await orderQueries.GetCardTypesAsync();
        return TypedResults.Ok(cardTypes);
    }

    public static async Task<OrderDraftDTO> CreateOrderDraftAsync(CreateOrderDraftCommand command, [AsParameters] OrderServices services)
    {
        services.Logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.BuyerId),
            command.BuyerId,
            command);

        return await services.Mediator.Send(command);
    }

    public static async Task<Results<Ok, BadRequest<string>>> CreateOrderAsync(
        [FromHeader(Name = "x-requestid")] Guid requestId,
        OrderDto request,
        [AsParameters] OrderServices services)
    {

        //mask the credit card number

        services.Logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId}",
            request.GetGenericTypeName(),
            nameof(request.UserId),
            request.UserId); //don't log the request as it has CC number

        if (requestId == Guid.Empty)
        {
            services.Logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", request);
            return TypedResults.BadRequest("RequestId is missing.");
        }

        using (services.Logger.BeginScope(new List<KeyValuePair<string, object>> { new("IdentifiedCommandId", requestId) }))
        {
            string maskedCCNumber = request.CardNumber[^4..].PadLeft(request.CardNumber.Length, 'X');
            CreateOrderCommand createOrderCommand = new(request.Items, request.UserId, request.UserName, request.City, request.Street,
                request.State, request.Country, request.ZipCode,
                maskedCCNumber, request.CardHolderName, request.CardExpiration,
                request.CardSecurityNumber, request.CardType);

            IdentifiedCommand<CreateOrderCommand, bool> requestCreateOrder = new(createOrderCommand, requestId);

            services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                requestCreateOrder.GetGenericTypeName(),
                nameof(requestCreateOrder.Id),
                requestCreateOrder.Id,
                requestCreateOrder);

            bool result = await services.Mediator.Send(requestCreateOrder);

            if (result)
            {
                services.Logger.LogInformation("CreateOrderCommand succeeded - RequestId: {RequestId}", requestId);
            }
            else
            {
                services.Logger.LogWarning("CreateOrderCommand failed - RequestId: {RequestId}", requestId);
            }

            return TypedResults.Ok();
        }
    }
}
