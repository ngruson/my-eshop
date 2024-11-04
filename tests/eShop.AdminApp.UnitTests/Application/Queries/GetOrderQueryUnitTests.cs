using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.ServiceInvocation.OrderingApiClient;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetOrderQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_success_when_order_exists(
        AdminApp.Application.Queries.Order.GetOrder.GetOrderQuery query,
        [Substitute, Frozen] IOrderingApiClient orderingApiClient,
        AdminApp.Application.Queries.Order.GetOrder.GetOrderQueryHandler sut,
        Ordering.Contracts.GetOrder.OrderDto order)
    {
        // Arrange

        orderingApiClient.GetOrder(query.ObjectId)
            .Returns(order);

        // Act

        Result<AdminApp.Application.Queries.Order.GetOrder.OrderViewModel> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await orderingApiClient.Received().GetOrder(query.ObjectId);
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_error_when_exception_is_thrown(
        AdminApp.Application.Queries.Order.GetOrder.GetOrderQuery query,
        [Substitute, Frozen] IOrderingApiClient orderingApiClient,
        AdminApp.Application.Queries.Order.GetOrder.GetOrderQueryHandler sut)
    {
        // Arrange

        orderingApiClient.GetOrder(Arg.Any<Guid>())
            .ThrowsAsync<Exception>();

        // Act

        Result<AdminApp.Application.Queries.Order.GetOrder.OrderViewModel> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await orderingApiClient.Received().GetOrder(Arg.Any<Guid>());
    }
}
