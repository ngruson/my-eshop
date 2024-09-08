using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Queries.GetOrders;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;
using NSubstitute.ExceptionExtensions;

namespace eShop.Ordering.UnitTests.Application.Queries;

public class GetOrdersQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task WhenOrdersExist_ReturnOrders(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        GetOrdersQueryHandler sut,
        GetOrdersQuery query,
        List<Order> orders,
        List<Buyer> buyers
    )
    {
        // Arrange

        for (int i = 0; i < orders.Count; i++)
        {
            orders[i].SetBuyer(buyers[i]);
        }

        orderRepository.ListAsync(default)
            .Returns(orders);

        // Act

        Result<List<OrderDto>> result = await sut.Handle(query, default);

        //Assert

        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.Count, orders.Count);        
    }

    [Theory, AutoNSubstituteData]
    internal async Task WhenOrdersDoNotExist_ReturnNotFound(
        GetOrdersQueryHandler sut,
        GetOrdersQuery query
    )
    {
        // Arrange

        // Act

        Result<List<OrderDto>> result = await sut.Handle(query, default);

        //Assert

        Assert.True(result.IsNotFound());
        Assert.Null(result.Value);
    }

    [Theory, AutoNSubstituteData]
    internal async Task WhenExceptionIsThrown_ReturnError(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        GetOrdersQueryHandler sut,
        GetOrdersQuery query
    )
    {
        // Arrange

        orderRepository.ListAsync(default)
            .ThrowsAsync<Exception>();

        // Act

        Result<List<OrderDto>> result = await sut.Handle(query, default);

        //Assert

        Assert.True(result.IsError());        
    }
}
