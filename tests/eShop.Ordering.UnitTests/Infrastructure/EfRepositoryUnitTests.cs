using eShop.Ordering.Infrastructure;
using eShop.Ordering.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace eShop.Ordering.UnitTests.Infrastructure;

public class EfRepositoryUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task begin_transaction(
        DbContextOptionsBuilder<OrderingContext> optionsBuilder,
        IMediator mediator)
    {
        // Arrange

        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
        optionsBuilder.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        OrderingContext context = new(optionsBuilder.Options, mediator);
        context.Database.EnsureCreated();

        EfRepository<Ordering.Domain.AggregatesModel.OrderAggregate.Order> efRepository = new(context);

        // Act

        await efRepository.BeginTransactionAsync();

        // Assert

        Assert.NotNull(context.GetCurrentTransaction());
    }

    [Theory, AutoNSubstituteData]
    public async Task commit_transaction(
        DbContextOptionsBuilder<OrderingContext> optionsBuilder,
        IMediator mediator)
    {
        // Arrange

        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
        optionsBuilder.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        OrderingContext context = new(optionsBuilder.Options, mediator);
        context.Database.EnsureCreated();

        EfRepository<Ordering.Domain.AggregatesModel.OrderAggregate.Order> efRepository = new(context);
        await efRepository.BeginTransactionAsync();

        // Act

        await efRepository.CommitTransactionAsync();

        // Assert

        Assert.Null(context.GetCurrentTransaction());
    }

    [Theory, AutoNSubstituteData]
    public async Task execute_in_transaction(
        DbContextOptionsBuilder<OrderingContext> optionsBuilder,
        IMediator mediator)
    {
        // Arrange

        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
        optionsBuilder.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        OrderingContext context = new(optionsBuilder.Options, mediator);
        context.Database.EnsureCreated();

        EfRepository<Ordering.Domain.AggregatesModel.OrderAggregate.Order> efRepository = new(context);

        // Act

        bool called = false;
        async Task func(Guid _) { await Task.Run(() => called = true); };
        await efRepository.ExecuteInTransactionAsync(func);

        // Assert

        Assert.True(called);
        Assert.Null(context.GetCurrentTransaction());
    }
}
