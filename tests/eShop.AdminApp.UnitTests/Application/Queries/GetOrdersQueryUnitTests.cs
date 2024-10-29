using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.Order.GetOrders;
using eShop.Ordering.Contracts;
using eShop.Ordering.Contracts.GetOrders;
using eShop.ServiceInvocation.OrderingService;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetOrdersQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerCreated(
        GetOrdersQuery query,
        [Substitute, Frozen] IOrderingService orderingService,
        GetOrdersQueryHandler sut,
        OrderDto[] orders)
    {
        // Arrange

        orderingService.GetOrders()
            .Returns(orders);

        // Act

        Result<List<OrderViewModel>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await orderingService.Received().GetOrders();
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetOrdersQuery query,
        [Substitute, Frozen] IOrderingService orderingService,
        GetOrdersQueryHandler sut)
    {
        // Arrange

        orderingService.GetOrders()
            .ThrowsAsync<Exception>();

        // Act

        Result<List<OrderViewModel>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await orderingService.Received().GetOrders();
    }
}
