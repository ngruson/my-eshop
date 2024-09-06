using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Queries;
using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;

namespace Ordering.UnitTests.Application.Queries;

public class OrderQueriesUnitTests
{
    public class GetOrder
    {
        [Theory, AutoNSubstituteData]
        internal async Task GetOrdersById_WhenOrderFound_ReturnsOrder(
        [Substitute, Frozen] IRepository<eShop.Ordering.Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
        OrderQueries sut,
        eShop.Ordering.Domain.AggregatesModel.OrderAggregate.Order order
        )
        {
            // Arrange

            orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>()).Returns(order);

            // Act

            var result = await sut.GetOrderAsync(order.Id);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(order.Id, result.OrderNumber);
        }

        [Theory, AutoNSubstituteData]
        internal async Task GetOrdersById_WhenOrderNotFound_ThrowsKeyNotFoundException(
            OrderQueries sut,
            eShop.Ordering.Domain.AggregatesModel.OrderAggregate.Order order
            )
        {
            // Arrange

            // Act

            async Task func() => await sut.GetOrderAsync(order.Id);

            // Assert

            await Assert.ThrowsAsync<KeyNotFoundException>(func);
        }
    }

    public class GetOrdersFromUser
    {
        [Theory, AutoNSubstituteData]
        internal async Task WhenOrdersFound_ReturnsOrders(
        [Substitute, Frozen] IRepository<eShop.Ordering.Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
        OrderQueries sut,
        List<eShop.Ordering.Domain.AggregatesModel.OrderAggregate.Order> orders,
        string userId
        )
        {
            // Arrange

            orderRepository.ListAsync(Arg.Any<GetOrdersFromUserSpecification>()).Returns(orders);

            // Act

            var result = await sut.GetOrdersFromUserAsync(userId);

            // Assert

            Assert.Equal(orders.Count, result.Count());
        }
    }

    public class GetCardTypes
    {
        [Theory, AutoNSubstituteData]
        internal async Task WhenCardTypesFound_ReturnsCardTypes(
        [Substitute, Frozen] IRepository<eShop.Ordering.Domain.AggregatesModel.BuyerAggregate.CardType> cardTypeRepository,
        OrderQueries sut,
        List<eShop.Ordering.Domain.AggregatesModel.BuyerAggregate.CardType> cardTypes
        )
        {
            // Arrange

            cardTypeRepository.ListAsync().Returns(cardTypes);

            // Act

            var result = await sut.GetCardTypesAsync();

            // Assert

            Assert.Equal(cardTypes.Count, result.Count());
        }
    }
}
