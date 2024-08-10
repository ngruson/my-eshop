namespace Ordering.UnitTests.Application;

using Microsoft.AspNetCore.Http.HttpResults;
using eShop.Ordering.API.Application.Queries;
using Order = eShop.Ordering.API.Application.Queries.Order;
using eShop.Ordering.API;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using NSubstitute.ExceptionExtensions;

public class OrdersApiUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task Cancel_order_with_requestId_success(
        [Substitute, Frozen] IMediator mediator,
        OrderServices orderServices)
    {
        // Arrange

        mediator.Send(Arg.Any<IdentifiedCommand<CancelOrderCommand, bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        
        var result = await OrdersApi.CancelOrderAsync(Guid.NewGuid(), new CancelOrderCommand(1), orderServices);

        // Assert
        Assert.IsType<Ok>(result.Result);
    }

    [Theory, AutoNSubstituteData]
    public async Task Cancel_order_bad_request(
        [Substitute, Frozen] IMediator mediator,
        OrderServices orderServices)
    {
        // Arrange

        mediator.Send(Arg.Any<IdentifiedCommand<CancelOrderCommand, bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act

        var result = await OrdersApi.CancelOrderAsync(Guid.Empty, new CancelOrderCommand(1), orderServices);

        // Assert

        Assert.IsType<BadRequest<string>>(result.Result);
    }

    [Theory, AutoNSubstituteData]
    public async Task Ship_order_with_requestId_success(
        [Substitute, Frozen] IMediator mediator,
        OrderServices orderServices)
    {
        // Arrange

        mediator.Send(Arg.Any<IdentifiedCommand<ShipOrderCommand, bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        
        var result = await OrdersApi.ShipOrderAsync(Guid.NewGuid(), new ShipOrderCommand(1), orderServices);

        // Assert

        Assert.IsType<Ok>(result.Result);
    }

    [Theory, AutoNSubstituteData]
    public async Task Ship_order_bad_request(
        [Substitute, Frozen] IMediator mediator,
        OrderServices orderServices)
    {
        // Arrange

        mediator.Send(Arg.Any<IdentifiedCommand<CreateOrderCommand, bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        
        var result = await OrdersApi.ShipOrderAsync(Guid.Empty, new ShipOrderCommand(1), orderServices);

        // Assert

        Assert.IsType<BadRequest<string>>(result.Result);
    }

    [Theory, AutoNSubstituteData]
    public async Task Get_orders_success(
        //[Substitute, Frozen] IMediator mediator,
        OrderServices orderServices)
    {
        // Arrange

        // Act
        
        var result = await OrdersApi.GetOrdersByUserAsync(orderServices);

        // Assert
        Assert.IsType<Ok<IEnumerable<OrderSummary>>>(result);
    }

    [Theory, AutoNSubstituteData]
    public async Task Get_order_success(
        [Substitute, Frozen] OrderServices orderServices,
        int orderId,
        Order order)
    {
        // Arrange

        orderServices.Queries.GetOrderAsync(orderId)
            .Returns(Task.FromResult(order));

        // Act

        var result = await OrdersApi.GetOrderAsync(orderId, orderServices);

        // Assert
        Assert.IsType<Ok<Order>>(result.Result);
        Assert.Equal(order, ((Ok<Order>)result.Result).Value);
    }

    [Theory, AutoNSubstituteData]
    public async Task Get_order_fails(
        [Substitute, Frozen] OrderServices orderServices,
        int orderId)
    {
        // Arrange

        orderServices.Queries.GetOrderAsync(orderId).ThrowsAsync<Exception>();

        // Act

        var result = await OrdersApi.GetOrderAsync(orderId, orderServices);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory, AutoNSubstituteData]
    public async Task Get_cardTypes_success(
        [Substitute, Frozen] IOrderQueries orderQueries,
        IEnumerable<CardType> cardTypes)
    {
        // Arrange
        
        orderQueries.GetCardTypesAsync()
            .Returns(Task.FromResult(cardTypes));

        // Act
        var result = await OrdersApi.GetCardTypesAsync(orderQueries);

        // Assert
        Assert.IsType<Ok<IEnumerable<CardType>>>(result);
        Assert.Equal(cardTypes, result.Value);
    }
}
