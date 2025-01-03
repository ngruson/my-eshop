using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.ServiceInvocation.WebAppApiClient;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Refit;

public class WebAppApiClientUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task notify_order_status_change(
            [Substitute, Frozen] IWebAppApi webAppApi,
            WebAppApiClient.Refit.WebAppApiClient sut,
            string buyerIdentityGuid)
    {
        // Arrange

        // Act

        await sut.NotifyOrderStatusChange(buyerIdentityGuid);

        // Assert

        await webAppApi.Received().NotifyOrderStatusChange(buyerIdentityGuid);
    }
}
