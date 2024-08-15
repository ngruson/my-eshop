using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;

namespace Ordering.UnitTests.Application.Commands;
public class ShipOrderCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task WhenOrderIsPaid_ShipOrder(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       ShipOrderCommandHandler sut,
       ShipOrderCommand command,
       Order order)
    {
        // Arrange

        order.SetAwaitingValidationStatus();
        order.SetStockConfirmedStatus();
        order.SetPaidStatus();

        orderRepository.GetByIdAsync(command.OrderNumber, default)
            .Returns(order);

        //Act

        await sut.Handle(command, default);

        //Assert

        Assert.Equal(OrderStatus.Shipped, order.OrderStatus);

        await orderRepository.Received().SaveEntitiesAsync(default);
    }

    [Theory, AutoNSubstituteData]
    public async Task WhenOrderIsNotPaid_ThrowDomainException(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       ShipOrderCommandHandler sut,
       ShipOrderCommand command,
       Order order)
    {
        // Arrange

        orderRepository.GetByIdAsync(command.OrderNumber, default)
            .Returns(order);

        //Act

        async Task<bool> func() => await sut.Handle(command, default);

        //Assert

        await Assert.ThrowsAsync<OrderingDomainException>((Func<Task<bool>>)func);
    }

    [Theory, AutoNSubstituteData]
    public async Task WhenOrderDoesNotExist_ReturnFalse(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       ShipOrderCommandHandler sut,
       ShipOrderCommand command)
    {
        // Arrange

        //Act

        bool result = await sut.Handle(command, default);

        //Assert

        Assert.False(result);

        await orderRepository.DidNotReceive().SaveEntitiesAsync(default);
    }
}
