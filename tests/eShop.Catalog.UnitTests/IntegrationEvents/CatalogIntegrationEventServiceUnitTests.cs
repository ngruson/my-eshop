using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.IntegrationEvents;
using eShop.Catalog.API.Model;
using eShop.EventBus.Abstractions;
using eShop.EventBus.Events;
using eShop.IntegrationEventLogEF.Services;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.API.UnitTests.IntegrationEvents;
public class CatalogIntegrationEventServiceUnitTests
{
    public class PublishThroughEventBus
    {
        [Theory, AutoNSubstituteData]
        internal async Task publish_ok(
            [Substitute, Frozen] IIntegrationEventLogService integrationEventLogService,
            [Substitute, Frozen] IEventBus eventBus,
            CatalogIntegrationEventService sut,
            IntegrationEvent integrationEvent
        )
        {
            // Arrange

            // Act

            await sut.PublishThroughEventBusAsync(integrationEvent, default);

            // Assert

            await integrationEventLogService.Received().MarkEventAsInProgressAsync(integrationEvent.Id, default);
            await eventBus.Received().PublishAsync(integrationEvent, default);
            await integrationEventLogService.Received().MarkEventAsPublishedAsync(integrationEvent.Id, default);

            await integrationEventLogService.DidNotReceive().MarkEventAsFailedAsync(integrationEvent.Id, default);
        }

        [Theory, AutoNSubstituteData]
        internal async Task publish_failed(
            [Substitute, Frozen] IIntegrationEventLogService integrationEventLogService,
            [Substitute, Frozen] IEventBus eventBus,
            CatalogIntegrationEventService sut,
            IntegrationEvent integrationEvent
        )
        {
            // Arrange

            eventBus.PublishAsync(integrationEvent, default)
                .ThrowsAsync<Exception>();

            // Act

            await sut.PublishThroughEventBusAsync(integrationEvent, default);

            // Assert

            await integrationEventLogService.Received().MarkEventAsInProgressAsync(integrationEvent.Id, default);
            await eventBus.Received().PublishAsync(integrationEvent, default);
            await integrationEventLogService.DidNotReceive().MarkEventAsPublishedAsync(integrationEvent.Id, default);

            await integrationEventLogService.Received().MarkEventAsFailedAsync(integrationEvent.Id, default);
        }
    }

    public class SaveEventAndDbChanges
    {
        [Theory, AutoNSubstituteData]
        internal async Task save_event_ok(
            [Substitute, Frozen] IRepository<CatalogItem> repository,
            CatalogIntegrationEventService sut,
            IntegrationEvent integrationEvent,
            Func<Task> func
        )
        {
            // Arrange

            // Act

            await sut.SaveEventAndDbChangesAsync(repository, integrationEvent, func, default);

            // Assert

            await repository.Received().ExecuteInTransactionAsync(Arg.Any<Func<Guid, Task>>(), default);
        }
    }
}
