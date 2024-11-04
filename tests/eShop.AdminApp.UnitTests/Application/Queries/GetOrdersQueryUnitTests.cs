using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.Contracts.GetOrders;
using eShop.ServiceInvocation.OrderingApiClient;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetOrdersQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_success_when_orders_exist(
        AdminApp.Application.Queries.Order.GetOrders.GetOrdersQuery query,
        [Substitute, Frozen] IOrderingApiClient orderingApiClient,
        AdminApp.Application.Queries.Order.GetOrders.GetOrdersQueryHandler sut,
        OrderDto[] orders)
    {
        // Arrange

        orderingApiClient.GetOrders()
            .Returns(orders);

        // Act

        Result<List<AdminApp.Application.Queries.Order.GetOrders.OrderViewModel>> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await orderingApiClient.Received().GetOrders();
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_error_when_exception_is_thrown(
        AdminApp.Application.Queries.Order.GetOrders.GetOrdersQuery query,
        [Substitute, Frozen] IOrderingApiClient orderingApiClient,
        AdminApp.Application.Queries.Order.GetOrders.GetOrdersQueryHandler sut)
    {
        // Arrange

        orderingApiClient.GetOrders()
            .ThrowsAsync<Exception>();

        // Act

        Result<List<AdminApp.Application.Queries.Order.GetOrders.OrderViewModel>> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await orderingApiClient.Received().GetOrders();
    }
}
