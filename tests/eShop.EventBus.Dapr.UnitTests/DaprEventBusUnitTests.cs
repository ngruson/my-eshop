using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Dapr.Client;
using eShop.EventBus.Abstractions;
using eShop.EventBus.Events;
using eShop.Shared.Features;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.EventBus.Dapr.UnitTests;

public class DaprEventBusUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_success_when_event_is_published(
        [Frozen, Substitute] IOptions<FeaturesConfiguration> featuresConfiguration,
        [Frozen, Substitute] IOptions<EventBusSubscriptionInfo> subscriptionOptions,
        FeaturesConfiguration features,
        EventBusSubscriptionInfo eventBusSubscriptionInfo,
        DaprEventBus sut,
        TestIntegrationEvent integrationEvent)
    {
        // Arrange

        featuresConfiguration.Value.Returns(features);
        subscriptionOptions.Value.Returns(eventBusSubscriptionInfo);

        // Act

        Result result = await sut.PublishAsync(integrationEvent, default);

        // Assert

        Assert.True(result.IsSuccess);
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_error_when_exception_is_thrown(
        [Frozen, Substitute] DaprClient daprClient,
        DaprEventBus sut,
        IntegrationEvent integrationEvent)
    {
        // Arrange

        daprClient.PublishEventAsync(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<IntegrationEvent>(),
            Arg.Any<Dictionary<string, string>>(),
            default)
        .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.PublishAsync(integrationEvent, default);

        // Assert

        Assert.True(result.IsError());
    }
}
