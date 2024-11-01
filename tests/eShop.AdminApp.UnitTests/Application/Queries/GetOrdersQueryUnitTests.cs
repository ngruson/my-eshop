using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.Order.GetOrders;
using eShop.Ordering.Contracts;
using eShop.Ordering.Contracts.GetOrders;
using eShop.ServiceInvocation.OrderingApiClient;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetOrdersQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerCreated(
        GetOrdersQuery query,
        [Substitute, Frozen] IOrderingApiClient orderingApiClient,
        GetOrdersQueryHandler sut,
        OrderDto[] orders)
    {
        // Arrange

        orderingApiClient.GetOrders()
            .Returns(orders);

        // Act

        Result<List<OrderViewModel>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await orderingApiClient.Received().GetOrders();
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetOrdersQuery query,
        [Substitute, Frozen] IOrderingApiClient orderingApiClient,
        GetOrdersQueryHandler sut)
    {
        // Arrange

        orderingApiClient.GetOrders()
            .ThrowsAsync<Exception>();

        // Act

        Result<List<OrderViewModel>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await orderingApiClient.Received().GetOrders();
    }
}
