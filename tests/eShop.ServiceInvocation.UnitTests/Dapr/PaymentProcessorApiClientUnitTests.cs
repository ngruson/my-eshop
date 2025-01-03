using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Dapr.Client;
using eShop.PaymentProcessor.Contracts;
using eShop.ServiceInvocation.Auth;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Dapr;

public class PaymentProcessorApiClientUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task process_payment(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            PaymentProcessorApiClient.Dapr.PaymentProcessorApiClient sut,            
            PaymentStatus paymentStatus,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
    {
        // Arrange

        accessTokenAccessor.GetAccessToken().Returns(accessToken);
        accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

        daprClient.CreateInvokeMethodRequest(
            HttpMethod.Post,
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
                .Returns(httpRequestMessage);

        daprClient.InvokeMethodAsync<PaymentStatus>(httpRequestMessage)
            .Returns(paymentStatus);

        // Act

        PaymentStatus actual = await sut.ProcessPayment();

        // Assert

        Assert.Equivalent(actual, paymentStatus);
    }
}
