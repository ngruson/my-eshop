using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Queries;
using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;

namespace eShop.Ordering.UnitTests.Application.Queries;

public class OrderQueriesUnitTests
{
    public class GetOrder
    {
        [Theory, AutoNSubstituteData]
        internal async Task GetOrdersById_WhenOrderFound_ReturnsOrder(
        [Substitute, Frozen] IRepository<Ordering.Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
        OrderQueries sut,
        Ordering.Domain.AggregatesModel.OrderAggregate.Order order
        )
        {
            // Arrange

            orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>()).Returns(order);

            // Act

            Order result = await sut.GetOrderAsync(order.ObjectId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(order.Id, result.OrderNumber);
        }

        [Theory, AutoNSubstituteData]
        internal async Task GetOrdersById_WhenOrderNotFound_ThrowsKeyNotFoundException(
            OrderQueries sut,
            Ordering.Domain.AggregatesModel.OrderAggregate.Order order
            )
        {
            // Arrange

            // Act

            async Task func() => await sut.GetOrderAsync(order.ObjectId);

            // Assert

            await Assert.ThrowsAsync<KeyNotFoundException>(func);
        }
    }

    public class GetOrdersFromUser
    {
        [Theory, AutoNSubstituteData]
        internal async Task WhenOrdersFound_ReturnsOrders(
        [Substitute, Frozen] IRepository<Ordering.Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
        OrderQueries sut,
        List<Ordering.Domain.AggregatesModel.OrderAggregate.Order> orders,
        string userId
        )
        {
            // Arrange

            orderRepository.ListAsync(Arg.Any<GetOrdersFromUserSpecification>()).Returns(orders);

            // Act

            IEnumerable<OrderSummary> result = await sut.GetOrdersFromUserAsync(userId);

            // Assert

            Assert.Equal(orders.Count, result.Count());
        }
    }

    public class GetCardTypes
    {
        [Theory, AutoNSubstituteData]
        internal async Task WhenCardTypesFound_ReturnsCardTypes(
        [Substitute, Frozen] IRepository<CardType> cardTypeRepository,
        OrderQueries sut,
        List<CardType> cardTypes
        )
        {
            // Arrange

            cardTypeRepository.ListAsync().Returns(cardTypes);

            // Act

            IEnumerable<Contracts.GetCardTypes.CardTypeDto> result = await sut.GetCardTypesAsync();

            // Assert

            Assert.Equal(cardTypes.Count, result.Count());
        }
    }
}
