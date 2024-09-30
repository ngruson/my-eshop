using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.GetOrders;
using eShop.Ordering.Contracts;
using eShop.Ordering.Contracts.GetOrders;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetOrdersQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerCreated(
        GetOrdersQuery query,
        [Substitute, Frozen] IOrderingApi orderingApi,
        GetOrdersQueryHandler sut,
        OrderDto[] orders)
    {
        // Arrange

        orderingApi.GetOrders()
            .Returns(orders);

        // Act

        Result<List<OrderViewModel>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await orderingApi.Received().GetOrders();
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetOrdersQuery query,
        [Substitute, Frozen] IOrderingApi orderingApi,
        GetOrdersQueryHandler sut)
    {
        // Arrange

        orderingApi.GetOrders()
            .ThrowsAsync<Exception>();

        // Act

        Result<List<OrderViewModel>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await orderingApi.Received().GetOrders();
    }
}
