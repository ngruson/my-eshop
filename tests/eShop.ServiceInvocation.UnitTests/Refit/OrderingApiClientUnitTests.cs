using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Ordering.Contracts;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Refit;

public class OrderingApiClientUnitTests
{
    public class CancelOrder
    {
        [Theory, AutoNSubstituteData]
        public async Task cancel_order(
            [Substitute, Frozen] IOrderingApi orderingApi,
            OrderingApiClient.Refit.OrderingApiClient sut,
            Guid objectId
        )
        {
            // Arrange

            // Act

            await sut.Cancel(objectId);

            // Assert

            await orderingApi.Received().Cancel(objectId);
        }
    }

    public class ConfirmGracePeriod
    {
        [Theory, AutoNSubstituteData]
        public async Task confirm_grace_period(
            [Substitute, Frozen] IOrderingApi orderingApi,
            OrderingApiClient.Refit.OrderingApiClient sut,
            Guid objectId
        )
        {
            // Arrange

            // Act

            await sut.ConfirmGracePeriod(objectId);

            // Assert

            await orderingApi.Received().ConfirmGracePeriod(objectId);
        }
    }

    public class ConfirmStock
    {
        [Theory, AutoNSubstituteData]
        public async Task confirm_stock(
            [Substitute, Frozen] IOrderingApi orderingApi,
            OrderingApiClient.Refit.OrderingApiClient sut,
            Guid objectId
        )
        {
            // Arrange

            // Act

            await sut.ConfirmStock(objectId);

            // Assert

            await orderingApi.Received().ConfirmStock(objectId);
        }
    }

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

    public class GetOrder
    {
        [Theory, AutoNSubstituteData]
        public async Task return_order(
            [Substitute, Frozen] IOrderingApi orderingApi,
            OrderingApiClient.Refit.OrderingApiClient sut,
            Ordering.Contracts.GetOrder.OrderDto order
        )
        {
            // Arrange
            orderingApi.GetOrder(order.ObjectId)
                .Returns(order);

            // Act

            Ordering.Contracts.GetOrder.OrderDto actual = await sut.GetOrder(order.ObjectId);

            // Assert

            Assert.Equal(actual, order);
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

    public class Paid
    {
        [Theory, AutoNSubstituteData]
        public async Task paid(
            [Substitute, Frozen] IOrderingApi orderingApi,
            OrderingApiClient.Refit.OrderingApiClient sut,
            Guid objectId
        )
        {
            // Arrange

            // Act

            await sut.Paid(objectId);

            // Assert

            await orderingApi.Received().Paid(objectId);
        }
    }

    public class RejectStock
    {
        [Theory, AutoNSubstituteData]
        public async Task reject_stock(
            [Substitute, Frozen] IOrderingApi orderingApi,
            OrderingApiClient.Refit.OrderingApiClient sut,
            Guid objectId,
            Guid[] orderStockItems
        )
        {
            // Arrange

            // Act

            await sut.RejectStock(objectId, orderStockItems);

            // Assert

            await orderingApi.Received().RejectStock(objectId, orderStockItems);
        }
    }

    public class UpdateOrder
    {
        [Theory, AutoNSubstituteData]
        public async Task update_order(
            [Substitute, Frozen] IOrderingApi orderingApi,
            OrderingApiClient.Refit.OrderingApiClient sut,
            Guid objectId,
            Ordering.Contracts.UpdateOrder.OrderDto order
        )
        {
            // Arrange

            // Act

            await sut.UpdateOrder(objectId, order);

            // Assert

            await orderingApi.Received().UpdateOrder(objectId, order);
        }
    }
}
