using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;

namespace Ordering.UnitTests.Application.Commands;
public class SetAwaitingValidationOrderStatusCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task WhenStatusIsSubmitted_OrderIsAwaitingValidation(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       SetAwaitingValidationOrderStatusCommandHandler sut,
       SetAwaitingValidationOrderStatusCommand command,
       Order order)
    {
        // Arrange

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        //Act

        await sut.Handle(command, default);

        //Assert

        Assert.Equal(OrderStatus.AwaitingValidation, order.OrderStatus);

        await orderRepository.Received().SaveEntitiesAsync(default);
    }

    [Theory, AutoNSubstituteData]
    public async Task WhenStatusIsNotSubmitted_StatusIsNotChanged(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       SetAwaitingValidationOrderStatusCommandHandler sut,
       SetAwaitingValidationOrderStatusCommand command,
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

        Assert.NotEqual(OrderStatus.AwaitingValidation, order.OrderStatus);

        await orderRepository.Received().SaveEntitiesAsync(default);
    }

    [Theory, AutoNSubstituteData]
    public async Task WhenOrderDoesNotExist_ReturnFalse(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       SetAwaitingValidationOrderStatusCommandHandler sut,
       SetAwaitingValidationOrderStatusCommand command)
    {
        // Arrange

        //Act

        bool result = await sut.Handle(command, default);

        //Assert

        Assert.False(result);

        await orderRepository.DidNotReceive().SaveEntitiesAsync(default);
    }
}
