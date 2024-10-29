using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.Contracts;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using eShop.AdminApp.Application.Commands.Order.CreateOrder;
using eShop.ServiceInvocation.OrderingService;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class CreateOrderCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenOrderCreated(
        CreateOrderCommand command,
        [Substitute, Frozen] IOrderingService orderingService,
        CreateOrderCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await orderingService.Received().CreateOrder(command.RequestId, command.Dto);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        CreateOrderCommand command,
        [Substitute, Frozen] IOrderingService orderingService,
        CreateOrderCommandHandler sut)
    {
        // Arrange

        orderingService.CreateOrder(command.RequestId, command.Dto)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await orderingService.Received().CreateOrder(command.RequestId, command.Dto);
    }
}
