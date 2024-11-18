using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Commands.ShipOrder;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;
using NSubstitute.ExceptionExtensions;

namespace eShop.Ordering.UnitTests.Application.Commands;
public class ShipOrderCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task return_success_when_order_is_shipped(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       ShipOrderCommandHandler sut,
       ShipOrderCommand command,
       Order order)
    {
        // Arrange

        order.SetAwaitingValidationStatus();
        order.SetStockConfirmedStatus();
        order.SetPaidStatus();

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        //Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsSuccess);
        Assert.Equal(OrderStatus.Shipped, order.OrderStatus);

        await orderRepository.Received().UpdateAsync(order, default);
    }

    [Theory, AutoNSubstituteData]
    public async Task return_conflict_when_order_has_wrong_status(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       ShipOrderCommandHandler sut,
       ShipOrderCommand command,
       Order order)
    {
        // Arrange

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        //Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsConflict());        
    }

    [Theory, AutoNSubstituteData]
    public async Task return_not_found_when_order_does_not_exist(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       ShipOrderCommandHandler sut,
       ShipOrderCommand command)
    {
        // Arrange

        //Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsNotFound());

        await orderRepository.DidNotReceive().UpdateAsync(Arg.Any<Order>(), default);
    }

    [Theory, AutoNSubstituteData]
    public async Task return_error_when_exception_is_thrown(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       ShipOrderCommandHandler sut,
       ShipOrderCommand command)
    {
        // Arrange

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .ThrowsAsync<Exception>();

        //Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsError());

        await orderRepository.DidNotReceive().UpdateAsync(Arg.Any<Order>(), default);
    }
}
