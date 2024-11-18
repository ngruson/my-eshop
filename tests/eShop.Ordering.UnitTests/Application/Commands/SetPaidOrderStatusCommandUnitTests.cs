using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Commands.SetPaidOrderStatus;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;

namespace eShop.Ordering.UnitTests.Application.Commands;
public class SetPaidOrderStatusCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task WhenStatusIsStockConfirmed_OrderIsPaid(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       SetPaidOrderStatusCommandHandler sut,
       SetPaidOrderStatusCommand command,
       Order order)
    {
        // Arrange

        order.SetAwaitingValidationStatus();
        order.SetStockConfirmedStatus();

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        //Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsSuccess);
        Assert.Equal(OrderStatus.Paid, order.OrderStatus);

        await orderRepository.Received().UpdateAsync(order, default);
    }

    [Theory, AutoNSubstituteData]
    public async Task WhenStatusIsNotStockConfirmed_OrderIsNotPaid(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       SetPaidOrderStatusCommandHandler sut,
       SetPaidOrderStatusCommand command,
       Order order)
    {
        // Arrange

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        //Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsSuccess);
        Assert.NotEqual(OrderStatus.Paid, order.OrderStatus);

        await orderRepository.Received().UpdateAsync(order, default);
    }

    [Theory, AutoNSubstituteData]
    public async Task WhenOrderDoesNotExist_ReturnFalse(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       SetPaidOrderStatusCommandHandler sut,
       SetPaidOrderStatusCommand command)
    {
        // Arrange

        //Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.False(result.IsSuccess);

        await orderRepository.DidNotReceive().UpdateAsync(Arg.Any<Order>(), default);
    }
}
