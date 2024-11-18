using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;

namespace eShop.Ordering.UnitTests.Application.Commands;
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

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsSuccess);
        Assert.Equal(OrderStatus.AwaitingValidation, order.OrderStatus);
        await orderRepository.Received().UpdateAsync(order, default);
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

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsSuccess);
        Assert.NotEqual(OrderStatus.AwaitingValidation, order.OrderStatus);
        await orderRepository.Received().UpdateAsync(order, default);
    }

    [Theory, AutoNSubstituteData]
    public async Task return_not_found_when_order_does_not_exist(
       [Substitute, Frozen] IRepository<Order> orderRepository,
       SetAwaitingValidationOrderStatusCommandHandler sut,
       SetAwaitingValidationOrderStatusCommand command)
    {
        // Arrange

        //Act

        Result result = await sut.Handle(command, default);

        //Assert

        Assert.True(result.IsNotFound());
        await orderRepository.DidNotReceive().UpdateAsync(Arg.Any<Order>(), default);
    }
}
