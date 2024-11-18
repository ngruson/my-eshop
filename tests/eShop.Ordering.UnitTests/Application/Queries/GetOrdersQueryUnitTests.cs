using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Queries.GetOrders;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Contracts.GetOrders;
using eShop.Shared.Data;
using NSubstitute.ExceptionExtensions;

namespace eShop.Ordering.UnitTests.Application.Queries;

public class GetOrdersQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task WhenOrdersExist_ReturnOrders(
        [Substitute, Frozen] IRepository<Ordering.Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
        GetOrdersQueryHandler sut,
        GetOrdersQuery query,
        List<Ordering.Domain.AggregatesModel.OrderAggregate.Order> orders,
        List<Buyer> buyers
    )
    {
        // Arrange

        for (int i = 0; i < orders.Count; i++)
        {
            orders[i].SetBuyer(buyers[i]);
        }

        orderRepository.ListAsync(Arg.Any<GetOrdersSpecification>(), default)
            .Returns(orders);

        // Act

        Result<List<OrderDto>> result = await sut.Handle(query, default);

        //Assert

        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.Count, orders.Count);        
    }

    [Theory, AutoNSubstituteData]
    internal async Task WhenExceptionIsThrown_ReturnError(
        [Substitute, Frozen] IRepository<Ordering.Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
        GetOrdersQueryHandler sut,
        GetOrdersQuery query
    )
    {
        // Arrange

        orderRepository.ListAsync(Arg.Any<GetOrdersSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<List<OrderDto>> result = await sut.Handle(query, default);

        //Assert

        Assert.True(result.IsError());        
    }
}
