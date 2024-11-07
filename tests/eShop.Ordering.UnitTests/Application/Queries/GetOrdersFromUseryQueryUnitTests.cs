using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Queries.GetOrdersFromUser;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Contracts.GetOrdersFromUser;
using eShop.Shared.Data;
using NSubstitute.ExceptionExtensions;

namespace eShop.Ordering.UnitTests.Application.Queries;

public class GetOrdersFromUseryQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_success_when_orders_exist_for_user(
        [Substitute, Frozen] IIdentityService identityService,
        [Substitute, Frozen] IRepository<Ordering.Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
        GetOrdersFromUserQueryHandler sut,
        GetOrdersFromUserQuery query,
        List<Ordering.Domain.AggregatesModel.OrderAggregate.Order> orders,
        Guid userId
    )
    {
        // Arrange

        identityService.GetUserIdentity().Returns(userId);

        orderRepository.ListAsync(Arg.Any<GetOrdersFromUserSpecification>(), default)
            .Returns(orders);

        // Act

        Result<OrderDto[]> result = await sut.Handle(query, default);

        //Assert

        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.Length, orders.Count);
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_error_when_exception_is_thrown(
        [Substitute, Frozen] IRepository<Ordering.Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
        GetOrdersFromUserQueryHandler sut,
        GetOrdersFromUserQuery query
    )
    {
        // Arrange

        orderRepository.ListAsync(Arg.Any<GetOrdersFromUserSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<OrderDto[]> result = await sut.Handle(query, default);

        //Assert

        Assert.True(result.IsError());
    }
}
