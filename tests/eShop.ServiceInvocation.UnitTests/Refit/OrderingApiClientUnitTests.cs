using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Ordering.Contracts.GetOrders;
using eShop.Ordering.Contracts;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Refit;

public class OrderingApiClientUnitTests
{
    public class GetOrders
    {
        [Theory, AutoNSubstituteData]
        public async Task return_orders(
            [Substitute, Frozen] IOrderingApi orderingApi,
            OrderingApiClient.Refit.OrderingApiClient sut,
            Ordering.Contracts.GetOrders.OrderDto[] orders
        )
        {
            // Arrange
            orderingApi.GetOrders()
                .Returns(orders);

            // Act

            Ordering.Contracts.GetOrders.OrderDto[] actual = await sut.GetOrders();

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
            Ordering.Contracts.CreateOrder.OrderDto request
        )
        {
            // Arrange

            // Act

            await sut.CreateOrder(requestId, request);

            // Assert

            await orderingApi.Received().CreateOrder(requestId, request);
        }
    }

    public class DeleteOrder
    {
        [Theory, AutoNSubstituteData]
        public async Task return_orders(
            [Substitute, Frozen] IOrderingApi orderingApi,
            OrderingApiClient.Refit.OrderingApiClient sut,
            Guid objectId
        )
        {
            // Arrange

            // Act
            await sut.DeleteOrder(objectId);

            // Assert

            await orderingApi.Received().DeleteOrder(objectId);
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
