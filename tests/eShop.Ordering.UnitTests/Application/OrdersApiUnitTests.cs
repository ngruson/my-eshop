using eShop.Ordering.API.Application.Queries;

namespace Ordering.UnitTests.Application;

using Microsoft.AspNetCore.Http.HttpResults;
using eShop.Ordering.API.Application.Queries;
using Order = Order;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using NSubstitute.ExceptionExtensions;
using Microsoft.AspNetCore.Routing;
using eShop.Ordering.API.Apis;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Ordering.API.Application.Commands.CancelOrder;
using eShop.Ordering.API.Application.Commands.CreateOrder;
using eShop.Ordering.API.Application.Commands.CreateOrderDraft;
using eShop.Ordering.API.Application.Commands.ShipOrder;

public class OrdersApiUnitTests
{
    public class MapOrdersApi
    {
        [Theory, AutoNSubstituteData]
        public void success(
            IEndpointRouteBuilder routeBuilder)
        {
            // Arrange

            // Act

            routeBuilder = routeBuilder.MapOrdersApiV1();

            // Assert

            Assert.Single(routeBuilder.DataSources);
        }
    }
    public class CancelOrder
    {
        [Theory, AutoNSubstituteData]
        public async Task With_requestId_success(
        [Substitute, Frozen] IMediator mediator,
        OrderServices orderServices,
        CancelOrderCommand command,
        Guid requestId)
        {
            // Arrange

            mediator.Send(Arg.Any<IdentifiedCommand<CancelOrderCommand, bool>>(), default)
                .Returns(Task.FromResult(true));

            // Act

            Results<Ok, BadRequest<string>, ProblemHttpResult> result =
                await OrdersApi.CancelOrderAsync(requestId, command, orderServices);

            // Assert
            Assert.IsType<Ok>(result.Result);
        }

        [Theory, AutoNSubstituteData]
        public async Task With_empty_request_id_bad_request(
            OrderServices orderServices,
            CancelOrderCommand command)
        {
            // Arrange

            // Act

            Results<Ok, BadRequest<string>, ProblemHttpResult> result =
                await OrdersApi.CancelOrderAsync(Guid.Empty, command, orderServices);

            // Assert

            Assert.IsType<BadRequest<string>>(result.Result);
        }

        [Theory, AutoNSubstituteData]
        public async Task When_command_fails_problem(
            [Substitute, Frozen] IMediator mediator,
            OrderServices orderServices,
            CancelOrderCommand command,
            Guid requestId)
        {
            // Arrange

            mediator.Send(Arg.Any<IdentifiedCommand<CancelOrderCommand, bool>>(), default)
                .Returns(Task.FromResult(false));

            // Act

            Results<Ok, BadRequest<string>, ProblemHttpResult> result =
                await OrdersApi.CancelOrderAsync(requestId, command, orderServices);

            // Assert

            Assert.IsType<ProblemHttpResult>(result.Result);
        }
    }

    public class ShipOrder
    {
        [Theory, AutoNSubstituteData]
        public async Task With_requestId_success(
            [Substitute, Frozen] IMediator mediator,
            OrderServices orderServices,
            ShipOrderCommand command,
            Guid requestId)
        {
            // Arrange

            mediator.Send(Arg.Any<IdentifiedCommand<ShipOrderCommand, bool>>(), default)
                .Returns(Task.FromResult(true));

            // Act

            Results<Ok, BadRequest<string>, ProblemHttpResult> result =
                await OrdersApi.ShipOrderAsync(requestId, command, orderServices);

            // Assert

            Assert.IsType<Ok>(result.Result);
        }

        [Theory, AutoNSubstituteData]
        public async Task With_empty_request_id_bad_request(
            [Substitute, Frozen] IMediator mediator,
            OrderServices orderServices,
            ShipOrderCommand command)
        {
            // Arrange

            mediator.Send(Arg.Any<IdentifiedCommand<CreateOrderCommand, bool>>(), default)
                .Returns(Task.FromResult(true));

            // Act

            Results<Ok, BadRequest<string>, ProblemHttpResult> result =
                await OrdersApi.ShipOrderAsync(Guid.Empty, command, orderServices);

            // Assert

            Assert.IsType<BadRequest<string>>(result.Result);
        }

        [Theory, AutoNSubstituteData]
        public async Task When_command_fails_problem(
            [Substitute, Frozen] IMediator mediator,
            OrderServices orderServices,
            ShipOrderCommand command,
            Guid requestId)
        {
            // Arrange

            mediator.Send(Arg.Any<IdentifiedCommand<CancelOrderCommand, bool>>(), default)
                .Returns(Task.FromResult(false));

            // Act

            Results<Ok, BadRequest<string>, ProblemHttpResult> result =
                await OrdersApi.ShipOrderAsync(requestId, command, orderServices);

            // Assert

            Assert.IsType<ProblemHttpResult>(result.Result);
        }
    }

    [Theory, AutoNSubstituteData]
    public async Task Get_orders_success(
        //[Substitute, Frozen] IMediator mediator,
        OrderServices orderServices)
    {
        // Arrange

        // Act

        Ok<IEnumerable<OrderSummary>> result = await OrdersApi.GetOrdersByUserAsync(orderServices);

        // Assert
        Assert.IsType<Ok<IEnumerable<OrderSummary>>>(result);
    }

    [Theory, AutoNSubstituteData]
    public async Task Create_order_draft(
        [Substitute, Frozen] OrderServices orderServices,
        CreateOrderDraftCommand command,
        OrderDraftDTO dto)
    {
        // Arrange

        orderServices.Mediator.Send(command)
            .Returns(Task.FromResult(dto));

        // Act

        OrderDraftDTO result = await OrdersApi.CreateOrderDraftAsync(command, orderServices);

        // Assert

        Assert.Equal(dto, result);
    }

    public class CreateOrder
    {
        [Theory, AutoNSubstituteData]
        public async Task When_command_succeeds_return_ok(
            [Substitute, Frozen] OrderServices orderServices,
            OrderDto request,
            Guid requestId)
        {
            // Arrange

            orderServices.Mediator.Send(Arg.Any<IdentifiedCommand<CreateOrderCommand, bool>>())
                .Returns(Task.FromResult(true));

            // Act

            Results<Ok, BadRequest<string>> result = await OrdersApi.CreateOrderAsync(requestId, request, orderServices);

            // Assert

            Assert.IsType<Ok>(result.Result);
        }

        [Theory, AutoNSubstituteData]
        public async Task When_command_fails_return_ok(
            OrderServices orderServices,
            OrderDto request,
            Guid requestId)
        {
            // Arrange

            // Act

            Results<Ok, BadRequest<string>> result = await OrdersApi.CreateOrderAsync(requestId, request, orderServices);

            // Assert

            Assert.IsType<Ok>(result.Result);
        }

        [Theory, AutoNSubstituteData]
        public async Task With_empty_request_id_return_bad_request(
            OrderServices orderServices,
            OrderDto request)
        {
            // Arrange

            // Act

            Results<Ok, BadRequest<string>> result = await OrdersApi.CreateOrderAsync(Guid.Empty, request, orderServices);

            // Assert

            Assert.IsType<BadRequest<string>>(result.Result);
        }
    }

    [Theory, AutoNSubstituteData]
    public async Task Get_cardTypes_success(
        [Substitute, Frozen] IOrderQueries orderQueries,
        IEnumerable<CardTypeDto> cardTypes)
    {
        // Arrange
        
        orderQueries.GetCardTypesAsync()
            .Returns(Task.FromResult(cardTypes));

        // Act

        Ok<IEnumerable<CardTypeDto>> result = await OrdersApi.GetCardTypesAsync(orderQueries);

        // Assert

        Assert.Equal(cardTypes, result.Value);
    }
}
