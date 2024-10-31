using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Ordering.Contracts.GetOrders;
using eShop.Ordering.Contracts;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests;

public class OrderingServiceUnitTests
{
    public class GetOrders
    {
        [Theory, AutoNSubstituteData]
        public async Task return_orders(
            [Substitute, Frozen] IOrderingApi orderingApi,
            OrderingApiClient.Refit.OrderingApiClient sut,
            OrderDto[] orders
        )
        {
            // Arrange
            orderingApi.GetOrders()
                .Returns(orders);

            // Act

            OrderDto[] actual = await sut.GetOrders();

            // Assert

            Assert.Equal(actual, orders);
        }
    }

    public class CreateOrder
    {
        [Theory, AutoNSubstituteData]
        public async Task return_orders(
            [Substitute, Frozen] IOrderingApi orderingApi,
            OrderingApiClient.Refit.OrderingApiClient sut,
            Guid requestId,
            CreateOrderDto request
        )
        {
            // Arrange

            // Act

            await sut.CreateOrder(requestId, request);

            // Assert

            await orderingApi.Received().CreateOrder(requestId, request);
        }
    }

    public class GetCardTypes
    {
        [Theory, AutoNSubstituteData]
        public async Task return_cardTypes(
            [Substitute, Frozen] IOrderingApi orderingApi,
            OrderingApiClient.Refit.OrderingApiClient sut,
            CardTypeDto[] cardTypes
        )
        {
            // Arrange

            orderingApi.GetCardTypes()
                .Returns(cardTypes);

            // Act

            CardTypeDto[] actual = (await sut.GetCardTypes()).ToArray();

            // Assert

            Assert.Equal(actual, cardTypes);
        }
    }
}
