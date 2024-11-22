using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.EventBus.Abstractions;
using eShop.EventBus.Events;
using eShop.IntegrationEventLogEF;
using eShop.IntegrationEventLogEF.Services;
using eShop.Ordering.API.Application.IntegrationEvents;
using eShop.Ordering.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute.ExceptionExtensions;

namespace eShop.Ordering.UnitTests.Application.IntegrationEvents;
public class OrderingIntegrationEventServiceUnitTests
{
    public class PublishEventsThroughEventBus
    {
        [Theory, AutoNSubstituteData]
        public async Task WhenEventsToPublish_PublishTheEvents(
            [Substitute, Frozen] IIntegrationEventLogService eventLogService,
            [Substitute, Frozen] IEventBus eventBus,
            OrderingIntegrationEventService sut,
            Guid transactionId,
            List<IntegrationEventLogEntry> logEntries)
        {
            // Arrange

            eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId, default)
                .Returns(logEntries);

            //Act

            await sut.PublishEventsThroughEventBusAsync(transactionId, default);

            //Assert

            await eventLogService.Received(logEntries.Count).MarkEventAsInProgressAsync(Arg.Any<Guid>(), default);
            await eventBus.Received(logEntries.Count).PublishAsync(Arg.Any<IntegrationEvent>(), default);
            await eventLogService.Received(logEntries.Count).MarkEventAsPublishedAsync(Arg.Any<Guid>(), default);
        }

        [Theory, AutoNSubstituteData]
        public async Task WhenPublishFails_MarkEventAsFailed(
            [Substitute, Frozen] IIntegrationEventLogService eventLogService,
            [Substitute, Frozen] IEventBus eventBus,
            OrderingIntegrationEventService sut,
            Guid transactionId,
            List<IntegrationEventLogEntry> logEntries)
        {
            // Arrange

            eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId, default)
                .Returns(logEntries);

            eventBus.PublishAsync(Arg.Any<IntegrationEvent>(), default)
                .ThrowsAsync<Exception>();

            //Act

            await sut.PublishEventsThroughEventBusAsync(transactionId, default);

            //Assert

            await eventLogService.Received(logEntries.Count).MarkEventAsInProgressAsync(Arg.Any<Guid>(), default);
            await eventBus.Received(logEntries.Count).PublishAsync(Arg.Any<IntegrationEvent>(), default);
            await eventLogService.Received(logEntries.Count).MarkEventAsFailedAsync(Arg.Any<Guid>(), default);

            await eventLogService.DidNotReceive().MarkEventAsPublishedAsync(Arg.Any<Guid>(), default);
        }
    }

    public class AddAndSaveEvent
    {
        [Theory, AutoNSubstituteData]
        public async Task SaveEvent(
            [Substitute, Frozen] IIntegrationEventLogService eventLogService,
            [Substitute, Frozen] OrderingContext orderingContext,
            OrderingIntegrationEventService sut,
            IntegrationEvent evt,
            IDbContextTransaction transaction)
        {
            // Arrange

            orderingContext.GetCurrentTransaction()
                .Returns(transaction);

            //Act

            await sut.AddAndSaveEventAsync(evt, default);

            //Assert

            await eventLogService.Received().SaveEventAsync(evt, Arg.Any<Guid>(), default);
        }
    }
}
