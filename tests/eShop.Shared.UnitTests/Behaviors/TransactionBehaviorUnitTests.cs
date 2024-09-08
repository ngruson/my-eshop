using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Shared.Behaviors;
using eShop.Shared.IntegrationEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Shared.UnitTests.Behaviors;

public class TransactionBehaviorUnitTests
{
    private static TestDbContext GetDatabase()
    {
        DbContextOptionsBuilder<TestDbContext> optionsBuilder = new();
        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");

        TestDbContext testDbContext = new(
            optionsBuilder.Options,
            Substitute.For<IMediator>());

        testDbContext.Database.EnsureCreated();

        return testDbContext;
    }

    [Theory, AutoNSubstituteData]
    internal async Task when_no_active_transaction_run_transaction_and_return_test_response(
        [Substitute, Frozen] IIntegrationEventService integrationEventService,
        TestRequest request,
        RequestHandlerDelegate<TestResponse?> requestHandlerDelegate)
    {
        // Arrange

        TestDbContext dbContext = GetDatabase();

        TransactionBehavior<TestRequest, TestResponse> sut = new(
            dbContext,
            integrationEventService,
            Substitute.For<ILogger<TransactionBehavior<TestRequest, TestResponse>>>());

        // Act

        TestResponse? result = await sut.Handle(request, requestHandlerDelegate, default);

        // Assert

        Assert.NotNull(result);

        await integrationEventService.Received().PublishEventsThroughEventBusAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Theory, AutoNSubstituteData]
    internal async Task when_active_transaction_return_test_response(
        [Substitute, Frozen] IIntegrationEventService integrationEventService,
        TestRequest request,
        RequestHandlerDelegate<TestResponse?> requestHandlerDelegate)
    {
        // Arrange

        TestDbContext dbContext = GetDatabase();

        await dbContext.BeginTransactionAsync();

        TransactionBehavior<TestRequest, TestResponse> sut = new(
            dbContext,
            Substitute.For<IIntegrationEventService>(),
            Substitute.For<ILogger<TransactionBehavior<TestRequest, TestResponse>>>());

        // Act

        TestResponse? result = await sut.Handle(request, requestHandlerDelegate, default);

        // Assert

        Assert.NotNull(result);
        await integrationEventService.DidNotReceive().PublishEventsThroughEventBusAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Theory, AutoNSubstituteData]
    internal async Task throw_exception(
        [Substitute, Frozen] IIntegrationEventService integrationEventService,
        TestRequest request,
        RequestHandlerDelegate<TestResponse?> requestHandlerDelegate)
    {
        // Arrange

        integrationEventService.PublishEventsThroughEventBusAsync(Arg.Any<Guid>(), default)
            .ThrowsAsync<Exception>();

        TestDbContext dbContext = GetDatabase();

        TransactionBehavior<TestRequest, TestResponse> sut = new(
            dbContext,
            integrationEventService,
            Substitute.For<ILogger<TransactionBehavior<TestRequest, TestResponse>>>());

        // Act

        async Task func() => await sut.Handle(request, requestHandlerDelegate, default);

        // Assert

        await Assert.ThrowsAsync<Exception>(func);
    }
}
