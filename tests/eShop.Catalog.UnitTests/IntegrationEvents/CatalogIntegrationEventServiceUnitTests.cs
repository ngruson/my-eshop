using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Infrastructure;
using eShop.Catalog.API.IntegrationEvents;
using eShop.EventBus.Abstractions;
using eShop.EventBus.Events;
using eShop.IntegrationEventLogEF;
using eShop.IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.IntegrationEvents;
public class CatalogIntegrationEventServiceUnitTests
{
    public class PublishThroughEventBus
    {
        [Theory, AutoNSubstituteData]
        internal async Task publish_ok(
            [Substitute, Frozen] IIntegrationEventLogService integrationEventLogService,
            [Substitute, Frozen] IEventBus eventBus,
            CatalogIntegrationEventService sut,
            Guid transactionId,
            IEnumerable<IntegrationEventLogEntry> pendingLogEvents
        )
        {
            // Arrange

            integrationEventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId, default)
                .Returns(pendingLogEvents);

            // Act            

            await sut.PublishEventsThroughEventBusAsync(transactionId, default);

            // Assert

            await integrationEventLogService.Received(pendingLogEvents.Count()).MarkEventAsInProgressAsync(Arg.Any<Guid>(), default);
            await eventBus.Received(pendingLogEvents.Count()).PublishAsync(Arg.Any<IntegrationEvent>(), default);
            await integrationEventLogService.Received(pendingLogEvents.Count()).MarkEventAsPublishedAsync(Arg.Any<Guid>(), default);

            await integrationEventLogService.DidNotReceive().MarkEventAsFailedAsync(Arg.Any<Guid>(), default);
        }

        [Theory, AutoNSubstituteData]
        internal async Task publish_failed(
            [Substitute, Frozen] IIntegrationEventLogService integrationEventLogService,
            [Substitute, Frozen] IEventBus eventBus,
            CatalogIntegrationEventService sut,
            Guid transactionId,
            IEnumerable<IntegrationEventLogEntry> pendingLogEvents
        )
        {
            // Arrange

            integrationEventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId, default)
                .Returns(pendingLogEvents);

            integrationEventLogService.MarkEventAsInProgressAsync(Arg.Any<Guid>(), default)
                .ThrowsAsync<Exception>();

            // Act

            await sut.PublishEventsThroughEventBusAsync(transactionId, default);

            // Assert

            await integrationEventLogService.Received(pendingLogEvents.Count()).MarkEventAsInProgressAsync(Arg.Any<Guid>(), default);
            await eventBus.DidNotReceive().PublishAsync(Arg.Any<IntegrationEvent>(), default);
            await integrationEventLogService.DidNotReceive().MarkEventAsPublishedAsync(Arg.Any<Guid>(), default);

            await integrationEventLogService.Received(pendingLogEvents.Count()).MarkEventAsFailedAsync(Arg.Any<Guid>(), default);
        }
    }

    public class AddAndSaveEvent
    {
        [Theory, AutoNSubstituteData]
        internal async Task add_event_ok(
            [Substitute, Frozen] IIntegrationEventLogService integrationEventLogService,
            [Substitute, Frozen] CatalogContext catalogContext,
            CatalogIntegrationEventService sut,
            IntegrationEvent integrationEvent,
            IDbContextTransaction transaction
        )
        {
            // Arrange

            catalogContext.GetCurrentTransaction()
                .Returns(transaction);

            // Act

            await sut.AddAndSaveEventAsync(integrationEvent, default);

            // Assert

            await integrationEventLogService.SaveEventAsync(integrationEvent, Arg.Any<Guid>(), default);
        }

        [Theory, AutoNSubstituteData]
        internal async Task do_not_save_event_when_not_in_transaction(
            [Substitute, Frozen] IIntegrationEventLogService integrationEventLogService,
            CatalogIntegrationEventService sut,
            IntegrationEvent integrationEvent
        )
        {
            // Arrange

            // Act

            await sut.AddAndSaveEventAsync(integrationEvent, default);

            // Assert

            await integrationEventLogService.DidNotReceive().SaveEventAsync(integrationEvent, Arg.Any<Guid>(), default);
        }
    }
}
