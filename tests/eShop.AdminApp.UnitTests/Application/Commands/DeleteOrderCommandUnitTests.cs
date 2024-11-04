using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Commands.Order.DeleteOrder;
using eShop.ServiceInvocation.OrderingApiClient;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class DeleteOrderCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerDeleted(
        DeleteOrderCommand command,
        [Substitute, Frozen] IOrderingApiClient orderingApiClient,
        DeleteOrderCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await orderingApiClient.Received().DeleteOrder(command.ObjectId);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        DeleteOrderCommand command,
        [Substitute, Frozen] IOrderingApiClient orderingApiClient,
        DeleteOrderCommandHandler sut)
    {
        // Arrange

        orderingApiClient.DeleteOrder(command.ObjectId)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await orderingApiClient.Received().DeleteOrder(command.ObjectId);
    }
}
