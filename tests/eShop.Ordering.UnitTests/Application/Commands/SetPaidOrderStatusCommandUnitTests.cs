using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;

namespace Ordering.UnitTests.Application.Commands;
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

        await sut.Handle(command, default);

        //Assert

        Assert.Equal(OrderStatus.Paid, order.OrderStatus);

        await orderRepository.Received().UnitOfWork.SaveEntitiesAsync(default);
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

        await sut.Handle(command, default);

        //Assert

        Assert.NotEqual(OrderStatus.Paid, order.OrderStatus);

        await orderRepository.Received().UnitOfWork.SaveEntitiesAsync(default);
    }

    [Theory, AutoNSubstituteData]
    public async Task WhenOrderDoesNotExist_ReturnFalse(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       SetPaidOrderStatusCommandHandler sut,
       SetPaidOrderStatusCommand command)
    {
        // Arrange

        //Act

        bool result = await sut.Handle(command, default);

        //Assert

        Assert.False(result);

        await orderRepository.DidNotReceive().UnitOfWork.SaveEntitiesAsync(default);
    }
}
