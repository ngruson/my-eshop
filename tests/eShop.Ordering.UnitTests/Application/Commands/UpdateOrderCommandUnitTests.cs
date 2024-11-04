using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Commands.UpdateOrder;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;
using NSubstitute.ExceptionExtensions;

namespace eShop.Ordering.UnitTests.Application.Commands;

public class UpdateOrderCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_success_when_order_is_updated(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        UpdateOrderCommandHandler sut,
        UpdateOrderCommand command,
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

        await orderRepository.Received().UpdateAsync(order, default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_not_found_when_order_does_not_exist(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        UpdateOrderCommandHandler sut,
        UpdateOrderCommand command,
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
    internal async Task return_error_when_exception_is_thrown(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        UpdateOrderCommandHandler sut,
        UpdateOrderCommand command,
        Order order
    )
    {
        // Arrange

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsError());

        await orderRepository.DidNotReceive().UpdateAsync(order, default);
    }
}
