using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Dapr.Client;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.ServiceInvocation.Auth;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Dapr;

public class WorkflowApiClientUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task confirm_grace_period(
        [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
        [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
        [Substitute, Frozen] DaprClient daprClient,
        WorkflowApiClient.Dapr.WorkflowApiClient sut,
        HttpRequestMessage httpRequestMessage,
        string workflowInstanceId,
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

        await sut.ConfirmGracePeriod(workflowInstanceId);

        // Assert

        await daprClient.Received().InvokeMethodAsync(httpRequestMessage);
    }

    [Theory, AutoNSubstituteData]
    public async Task create_order(
        [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
        [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
        [Substitute, Frozen] DaprClient daprClient,
        WorkflowApiClient.Dapr.WorkflowApiClient sut,
        HttpRequestMessage httpRequestMessage,
        OrderDto order,
        string accessToken)
    {
        // Arrange

        accessTokenAccessor.GetAccessToken().Returns(accessToken);
        accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

        daprClient.CreateInvokeMethodRequest(
            HttpMethod.Post,
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>(),
            order)
                .Returns(httpRequestMessage);

        // Act

        await sut.CreateOrder(order);

        // Assert

        await daprClient.Received().InvokeMethodAsync(httpRequestMessage);
    }
}
