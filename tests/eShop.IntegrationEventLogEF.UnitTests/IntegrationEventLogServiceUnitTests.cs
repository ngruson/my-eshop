using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.EventBus.Events;
using eShop.IntegrationEventLogEF.Services;
using eShop.IntegrationEventLogEF.Specifications;
using eShop.Shared.Data;
using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute;

namespace eShop.IntegrationEventLogEF.UnitTests;

public class IntegrationEventLogServiceUnitTests
{
    public class RetrieveEventLogsPendingToPublish
    {
        [Theory, AutoNSubstituteData]
        internal async Task when_pending_event_logs_exist_return_list(
            [Substitute, Frozen] IRepository<IntegrationEventLogEntry> repository,
            IntegrationEventLogService sut,
            Guid transactionId,
            List<TestIntegrationEvent> integrationEvents)
        {
            // Arrange

            List<IntegrationEventLogEntry> logEntries =
                integrationEvents.Select(_ => new IntegrationEventLogEntry(_, transactionId))
                .ToList();

            repository.ListAsync(Arg.Any<GetPendingEventLogsSpecification>(), default)
                .Returns(logEntries);

            sut.LoadEventTypes(typeof(IntegrationEventLogServiceUnitTests).Assembly);

            // Act

            IEnumerable<IntegrationEventLogEntry> result = await sut.RetrieveEventLogsPendingToPublishAsync(transactionId, default);

            // Assert

            Assert.Equal(logEntries.Count, result.Count());
        }

        [Theory, AutoNSubstituteData]
        public async Task when_no_pending_event_logs_return_empty_list(
            [Substitute, Frozen] IRepository<IntegrationEventLogEntry> repository,
            IntegrationEventLogService sut,
            Guid transactionId)
        {
            // Arrange

            repository.ListAsync(Arg.Any<GetPendingEventLogsSpecification>(), default)
                .Returns([]);

            // Act

            IEnumerable<IntegrationEventLogEntry> result = await sut.RetrieveEventLogsPendingToPublishAsync(transactionId, default);

            // Assert

            Assert.Empty(result);
        }
    }

    [Theory, AutoNSubstituteData]
    public async Task SaveEvent(
        [Substitute, Frozen] IRepository<IntegrationEventLogEntry> repository,
        IntegrationEventLogService sut,
        IntegrationEvent evt,
        IDbContextTransaction transaction)
    {
        // Arrange

        // Act

        await sut.SaveEventAsync(evt, transaction, default);

        // Assert

        await repository.Received().AddAsync(Arg.Any<IntegrationEventLogEntry>(), default);
    }

    public class MarkEventAsPublished
    {
        [Theory, AutoNSubstituteData]
        public async Task if_event_exists_update_event(
        [Substitute, Frozen] IRepository<IntegrationEventLogEntry> repository,
        IntegrationEventLogService sut,
        IntegrationEventLogEntry @event)
        {
            // Arrange

            repository.SingleOrDefaultAsync(Arg.Any<GetEventSpecification>())
                .Returns(@event);

            // Act

            await sut.MarkEventAsPublishedAsync(@event.EventId, default);

            // Assert

            Assert.Equal(EventStateEnum.Published, @event.State);
            await repository.Received().UpdateAsync(Arg.Any<IntegrationEventLogEntry>(), default);
        }

        [Theory, AutoNSubstituteData]
        public async Task if_event_does_not_exist_do_not_update_event(
        [Substitute, Frozen] IRepository<IntegrationEventLogEntry> repository,
        IntegrationEventLogService sut,
        IntegrationEventLogEntry @event)
        {
            // Arrange

            // Act

            await sut.MarkEventAsPublishedAsync(@event.EventId, default);

            // Assert

            await repository.DidNotReceive().UpdateAsync(Arg.Any<IntegrationEventLogEntry>(), default);
        }
    }

    public class MarkEventAsInProgress
    {
        [Theory, AutoNSubstituteData]
        public async Task if_event_exists_update_event(
        [Substitute, Frozen] IRepository<IntegrationEventLogEntry> repository,
        IntegrationEventLogService sut,
        IntegrationEventLogEntry @event)
        {
            // Arrange

            repository.SingleOrDefaultAsync(Arg.Any<GetEventSpecification>())
                .Returns(@event);

            // Act

            await sut.MarkEventAsInProgressAsync(@event.EventId, default);

            // Assert

            Assert.Equal(EventStateEnum.InProgress, @event.State);
            await repository.Received().UpdateAsync(Arg.Any<IntegrationEventLogEntry>(), default);
        }

        [Theory, AutoNSubstituteData]
        public async Task if_event_does_not_exist_do_not_update_event(
        [Substitute, Frozen] IRepository<IntegrationEventLogEntry> repository,
        IntegrationEventLogService sut,
        IntegrationEventLogEntry @event)
        {
            // Arrange

            // Act

            await sut.MarkEventAsPublishedAsync(@event.EventId, default);

            // Assert

            Assert.NotEqual(EventStateEnum.InProgress, @event.State);
            await repository.DidNotReceive().UpdateAsync(Arg.Any<IntegrationEventLogEntry>(), default);
        }
    }

    public class MarkEventAsFailed
    {
        [Theory, AutoNSubstituteData]
        public async Task if_event_exists_update_event(
        [Substitute, Frozen] IRepository<IntegrationEventLogEntry> repository,
        IntegrationEventLogService sut,
        IntegrationEventLogEntry @event)
        {
            // Arrange

            repository.SingleOrDefaultAsync(Arg.Any<GetEventSpecification>())
                .Returns(@event);

            // Act

            await sut.MarkEventAsFailedAsync(@event.EventId, default);

            // Assert

            Assert.Equal(EventStateEnum.PublishedFailed, @event.State);
            await repository.Received().UpdateAsync(Arg.Any<IntegrationEventLogEntry>(), default);
        }

        [Theory, AutoNSubstituteData]
        public async Task if_event_does_not_exist_do_not_update_event(
        [Substitute, Frozen] IRepository<IntegrationEventLogEntry> repository,
        IntegrationEventLogService sut,
        IntegrationEventLogEntry @event)
        {
            // Arrange

            // Act

            await sut.MarkEventAsPublishedAsync(@event.EventId, default);

            // Assert

            Assert.NotEqual(EventStateEnum.PublishedFailed, @event.State);
            await repository.DidNotReceive().UpdateAsync(Arg.Any<IntegrationEventLogEntry>(), default);
        }
    }
}
