using System.Text.Json;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Commands.SetStockRejectedOrderStatus;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;

namespace eShop.Ordering.UnitTests.Application.Commands;

public class SetStockRejectedOrderStatusCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task WhenStatusIsAwaitingValidation_OrderIsCancelled(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        SetStockRejectedOrderStatusCommandHandler sut,
        SetStockRejectedOrderStatusCommand command,
        Order order
    )
    {
        // Arrange

        order.SetAwaitingValidationStatus();

        orderRepository.GetByIdAsync(command.OrderNumber, default)
            .Returns(order);

        // Act

        await sut.Handle(command, default);

        //Assert

        Assert.Equal(OrderStatus.Cancelled, order.OrderStatus);
        await orderRepository.Received().UpdateAsync(order, default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task WhenStatusIsNotAwaitingValidation_OrderIsNotCancelled(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        SetStockRejectedOrderStatusCommandHandler sut,
        SetStockRejectedOrderStatusCommand command,
        Order order
    )
    {
        // Arrange

        orderRepository.GetByIdAsync(command.OrderNumber, default)
            .Returns(order);

        // Act

        await sut.Handle(command, default);

        //Assert

        Assert.NotEqual(OrderStatus.Cancelled, order.OrderStatus);

        await orderRepository.Received().UpdateAsync(order, default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task WhenOrderDoesNotExist_OrderIsNotUpdated(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        SetStockRejectedOrderStatusCommandHandler sut,
        SetStockRejectedOrderStatusCommand command
    )
    {
        // Arrange

        // Act

        bool result = await sut.Handle(command, default);

        //Assert

        Assert.False(result);

        await orderRepository.DidNotReceive().UpdateAsync(Arg.Any<Order>(), default);
    }

    [Theory, AutoNSubstituteData]
    public void Set_Stock_Rejected_OrderStatusCommand_Check_Serialization(
        SetStockRejectedOrderStatusCommand command
    )
    {
        // Arrange

        // Act

        string json = JsonSerializer.Serialize(command);
        SetStockRejectedOrderStatusCommand deserializedCommand = JsonSerializer.Deserialize<SetStockRejectedOrderStatusCommand>(json);

        //Assert

        Assert.Equal(command.OrderNumber, deserializedCommand.OrderNumber);

        //Assert for List<int>

        Assert.NotNull(deserializedCommand.OrderStockItems);
        Assert.Equal(command.OrderStockItems.Count, deserializedCommand.OrderStockItems.Count);

        for (int i = 0; i < command.OrderStockItems.Count; i++)
        {
            Assert.Equal(command.OrderStockItems[i], deserializedCommand.OrderStockItems[i]);
        }
    }
}
