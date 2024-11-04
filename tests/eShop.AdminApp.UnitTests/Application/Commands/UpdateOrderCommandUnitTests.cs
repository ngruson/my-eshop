using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Commands.Order.UpdateOrder;
using eShop.ServiceInvocation.OrderingApiClient;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class UpdateOrderCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_success_when_order_is_updated(
        UpdateOrderCommand command,
        [Substitute, Frozen] IOrderingApiClient orderingApiClient,
        UpdateOrderCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await orderingApiClient.Received().UpdateOrder(command.ObjectId, command.Dto);
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_error_when_exception_is_thrown(
        UpdateOrderCommand command,
        [Substitute, Frozen] IOrderingApiClient orderingApiClient,
        UpdateOrderCommandHandler sut)
    {
        // Arrange

        orderingApiClient.UpdateOrder(command.ObjectId, command.Dto)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await orderingApiClient.Received().UpdateOrder(command.ObjectId, command.Dto);
    }
}
