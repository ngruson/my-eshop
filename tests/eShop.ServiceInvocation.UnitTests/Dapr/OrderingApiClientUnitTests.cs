using eShop.Ordering.Contracts.GetCardTypes;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using NSubstitute;
using Dapr.Client;
using eShop.ServiceInvocation.Auth;

namespace eShop.ServiceInvocation.UnitTests.Dapr;

public class OrderingApiClientUnitTests
{
    public class GetOrders
    {
        [Theory, AutoNSubstituteData]
        public async Task return_orders(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            OrderingApiClient.Dapr.OrderingApiClient sut,
            Ordering.Contracts.GetOrders.OrderDto[] orders,
            HttpRequestMessage httpRequestMessage,
            string accessToken
        )
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<Ordering.Contracts.GetOrders.OrderDto[]>(httpRequestMessage)
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
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            OrderingApiClient.Dapr.OrderingApiClient sut,
            Ordering.Contracts.GetOrder.OrderDto order,
            HttpRequestMessage httpRequestMessage,
            string accessToken
        )
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<Ordering.Contracts.GetOrder.OrderDto>(httpRequestMessage)
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
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            OrderingApiClient.Dapr.OrderingApiClient sut,
            Guid requestId,
            Ordering.Contracts.CreateOrder.OrderDto dto,
            HttpRequestMessage httpRequestMessage,
            string accessToken
        )
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Post,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>(),
                dto)
            .Returns(httpRequestMessage);

            // Act

            await sut.CreateOrder(requestId, dto);

            // Assert

            await daprClient.Received().InvokeMethodAsync(httpRequestMessage);
        }
    }

    public class UpdateOrder
    {
        [Theory, AutoNSubstituteData]
        public async Task update_order(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            OrderingApiClient.Dapr.OrderingApiClient sut,
            Guid requestId,
            Ordering.Contracts.UpdateOrder.OrderDto dto,
            HttpRequestMessage httpRequestMessage,
            string accessToken
        )
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Put,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>(),
                dto)
            .Returns(httpRequestMessage);

            // Act

            await sut.UpdateOrder(requestId, dto);

            // Assert

            await daprClient.Received().InvokeMethodAsync(httpRequestMessage);
        }
    }

    public class DeleteOrder
    {
        [Theory, AutoNSubstituteData]
        public async Task delete_order(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            OrderingApiClient.Dapr.OrderingApiClient sut,
            Guid objectId,
            HttpRequestMessage httpRequestMessage,
            string accessToken
        )
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Delete,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            // Act

            await sut.DeleteOrder(objectId);

            // Assert

            await daprClient.Received().InvokeMethodAsync(httpRequestMessage);
        }

        public class GetCardTypes
        {
            [Theory, AutoNSubstituteData]
            public async Task return_cardTypes(
                [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
                [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
                [Substitute, Frozen] DaprClient daprClient,
                OrderingApiClient.Dapr.OrderingApiClient sut,
                HttpRequestMessage httpRequestMessage,
                string accessToken,
                CardTypeDto[] cardTypes)
            {
                // Arrange

                accessTokenAccessor.GetAccessToken().Returns(accessToken);
                accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

                daprClient.CreateInvokeMethodRequest(
                    HttpMethod.Get,
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
                .Returns(httpRequestMessage);

                daprClient.InvokeMethodAsync<CardTypeDto[]>(httpRequestMessage)
                    .Returns(cardTypes);

                // Act

                CardTypeDto[] actual = await sut.GetCardTypes();

                // Assert

                Assert.Equal(actual, cardTypes);
            }
        }
    }
}
