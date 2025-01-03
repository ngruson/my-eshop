using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Dapr.Client;
using eShop.ServiceInvocation.Auth;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Dapr;

public class WebAppApiClientUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task notify_order_status_change(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            WebAppApiClient.Dapr.WebAppApiClient sut,            
            HttpRequestMessage httpRequestMessage,
            string buyerIdentityGuid,
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

        // Act

        await sut.NotifyOrderStatusChange(buyerIdentityGuid);

        // Assert

        await daprClient.Received().InvokeMethodAsync(httpRequestMessage);
    }
}
