using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;

namespace Ordering.UnitTests.Application.Commands;
public class CancelOrderCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task WhenStatusIsNotPaidOrShipped_CancelOrder(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        CancelOrderCommandHandler sut,
        CancelOrderCommand command,
        Order order
    )
    {
        // Arrange

        orderRepository.GetByIdAsync(command.OrderNumber, default)
            .Returns(order);

        // Act

        await sut.Handle(command, default);

        //Assert

        Assert.Equal(OrderStatus.Cancelled, order.OrderStatus);

        await orderRepository.Received().UnitOfWork.SaveEntitiesAsync(default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task WhenStatusIsPaid_ThrowDomainException(
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

        orderRepository.GetByIdAsync(command.OrderNumber, default)
            .Returns(order);

        // Act

        async Task<bool> func() => await sut.Handle(command, default);

        //Assert

        await Assert.ThrowsAsync<OrderingDomainException>(func);

        await orderRepository.DidNotReceive().UnitOfWork.SaveEntitiesAsync(default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task WhenStatusIsShipped_ThrowDomainException(
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

        orderRepository.GetByIdAsync(command.OrderNumber, default)
            .Returns(order);

        // Act

        async Task<bool> func() => await sut.Handle(command, default);

        //Assert

        await Assert.ThrowsAsync<OrderingDomainException>(func);

        await orderRepository.DidNotReceive().UnitOfWork.SaveEntitiesAsync(default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task WhenOrderDoesNotExist_ReturnFalse(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        CancelOrderCommandHandler sut,
        CancelOrderCommand command
    )
    {
        // Arrange

        // Act

        var result = await sut.Handle(command, default);

        //Assert

        Assert.False(result);

        await orderRepository.DidNotReceive().UnitOfWork.SaveEntitiesAsync(default);
    }
}
