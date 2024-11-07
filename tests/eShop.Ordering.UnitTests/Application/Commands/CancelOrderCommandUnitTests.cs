using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Commands.CancelOrder;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;

namespace eShop.Ordering.UnitTests.Application.Commands;
public class CancelOrderCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_success_when_order_is_cancelled(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        CancelOrderCommandHandler sut,
        CancelOrderCommand command,
        Order order
    )
    {
        // Arrange

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        // Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsSuccess);
        Assert.Equal(OrderStatus.Cancelled, order.OrderStatus);

        await orderRepository.Received().UpdateAsync(order, default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_not_found_when_order_does_not_exist(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        CancelOrderCommandHandler sut,
        CancelOrderCommand command,
        Order order
    )
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsNotFound());

        await orderRepository.DidNotReceive().UpdateAsync(order, default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task throw_domain_exception_when_status_is_paid(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        CancelOrderCommandHandler sut,
        CancelOrderCommand command,
        Order order
    )
    {
        // Arrange

        order.SetAwaitingValidationStatus();
        order.SetStockConfirmedStatus();
        order.SetPaidStatus();

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        // Act

        async Task<Result> func() => await sut.Handle(command, default);

        //Assert

        await Assert.ThrowsAsync<OrderingDomainException>(func);

        await orderRepository.DidNotReceive().UpdateAsync(order, default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task throw_domain_exception_when_status_is_shipped(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        CancelOrderCommandHandler sut,
        CancelOrderCommand command,
        Order order
    )
    {
        // Arrange

        order.SetAwaitingValidationStatus();
        order.SetStockConfirmedStatus();
        order.SetPaidStatus();
        order.SetShippedStatus();

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        // Act

        async Task<Result> func() => await sut.Handle(command, default);

        //Assert

        await Assert.ThrowsAsync<OrderingDomainException>(func);

        await orderRepository.DidNotReceive().UpdateAsync(order, default);
    }

    
}
