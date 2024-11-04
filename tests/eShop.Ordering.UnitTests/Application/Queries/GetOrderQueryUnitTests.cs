using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Queries.GetOrder;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Contracts.GetOrder;
using eShop.Shared.Data;
using NSubstitute.ExceptionExtensions;

namespace eShop.Ordering.UnitTests.Application.Queries;

public class GetOrderQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_success_when_order_exists(
        [Substitute, Frozen] IRepository<Ordering.Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
        GetOrderQueryHandler sut,
        GetOrderQuery query,
        Ordering.Domain.AggregatesModel.OrderAggregate.Order order,
        Buyer buyer
    )
    {
        // Arrange

        order.SetBuyer(buyer);

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        // Act

        Result<OrderDto> result = await sut.Handle(query, default);

        //Assert

        Assert.True(result.IsSuccess);
        await orderRepository.Received().SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_not_found_when_order_does_not_exist(
        [Substitute, Frozen] IRepository<Ordering.Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
        GetOrderQueryHandler sut,
        GetOrderQuery query        
    )
    {
        // Arrange

        // Act

        Result<OrderDto> result = await sut.Handle(query, default);

        //Assert

        Assert.True(result.IsNotFound());
        await orderRepository.Received().SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task WhenExceptionIsThrown_ReturnError(
        [Substitute, Frozen] IRepository<Ordering.Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
        GetOrderQueryHandler sut,
        GetOrderQuery query
    )
    {
        // Arrange

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<OrderDto> result = await sut.Handle(query, default);

        //Assert

        Assert.True(result.IsError());  
    }
}
