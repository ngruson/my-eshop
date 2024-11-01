using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Ordering.Contracts.GetOrders;
using eShop.Ordering.Contracts;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using NSubstitute;
using eShop.Shared.Auth;
using Dapr.Client;
using eShop.MasterData.Contracts;

namespace eShop.ServiceInvocation.UnitTests.Dapr;

public class OrderingApiClientUnitTests
{
    public class GetOrders
    {
        [Theory, AutoNSubstituteData]
        public async Task return_orders(
            [Substitute, Frozen] AccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] DaprClient daprClient,
            OrderingApiClient.Dapr.OrderingApiClient sut,
            OrderDto[] orders,
            HttpRequestMessage httpRequestMessage,
            string accessToken
        )
        {
            // Arrange

            accessTokenAccessor.GetAccessTokenAsync()
                .Returns(accessToken);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
            .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<OrderDto[]>(httpRequestMessage)
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
            [Substitute, Frozen] AccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] DaprClient daprClient,
            OrderingApiClient.Dapr.OrderingApiClient sut,
            Guid requestId,
            CreateOrderDto dto,
            HttpRequestMessage httpRequestMessage,
            string accessToken
        )
        {
            // Arrange

            accessTokenAccessor.GetAccessTokenAsync()
                .Returns(accessToken);

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

    public class GetCardTypes
    {
        [Theory, AutoNSubstituteData]
        public async Task return_cardTypes(
            [Substitute, Frozen] AccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] DaprClient daprClient,
            OrderingApiClient.Dapr.OrderingApiClient sut,
            CardTypeDto[] cardTypes,
            HttpRequestMessage httpRequestMessage,
            string accessToken
        )
        {
            // Arrange

            accessTokenAccessor.GetAccessTokenAsync()
                .Returns(accessToken);

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
