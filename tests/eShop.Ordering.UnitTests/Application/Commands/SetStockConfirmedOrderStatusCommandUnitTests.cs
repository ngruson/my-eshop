using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Commands.SetStockConfirmedOrderStatus;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;

namespace eShop.Ordering.UnitTests.Application.Commands;
public class SetStockConfirmedOrderStatusCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task WhenOrderIsAwaitingValidation_ConfirmOrder(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       SetStockConfirmedOrderStatusCommandHandler sut,
       SetStockConfirmedOrderStatusCommand command,
       Order order)
    {
        // Arrange

        order.SetAwaitingValidationStatus();

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        //Act

        await sut.Handle(command, default);

        //Assert

        Assert.Equal(OrderStatus.StockConfirmed, order.OrderStatus);

        await orderRepository.Received().UpdateAsync(order, default);
    }

    [Theory, AutoNSubstituteData]
    public async Task WhenOrderIsNotAwaitingValidation_StatusIsNotChanged(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       SetStockConfirmedOrderStatusCommandHandler sut,
       SetStockConfirmedOrderStatusCommand command,
       Order order)
    {
        // Arrange

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        //Act

        await sut.Handle(command, default);

        //Assert

        Assert.NotEqual(OrderStatus.StockConfirmed, order.OrderStatus);

        await orderRepository.Received().UpdateAsync(order, default);
    }

    [Theory, AutoNSubstituteData]
    public async Task WhenOrderDoesNotExist_ReturnFalse(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       SetStockConfirmedOrderStatusCommandHandler sut,
       SetStockConfirmedOrderStatusCommand command)
    {
        // Arrange

        //Act

        bool result = await sut.Handle(command, default);

        //Assert

        Assert.False(result);

        await orderRepository.DidNotReceive().UpdateAsync(Arg.Any<Order>(), default);
    }
}
