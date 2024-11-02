using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute;

namespace eShop.Shared.UnitTests.Data;

public class eShopDbContextUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task SaveChangesAsync_ShouldReturnZeroGivenNoChanges(
        DbContextOptionsBuilder<TestDbContext> optionsBuilder,
        [Substitute, Frozen] IMediator mediator)
    {
        // Arrange

        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
        TestDbContext context = new(optionsBuilder.Options, mediator);
        context.Database.EnsureCreated();

        // Act

        int result = await context.SaveChangesAsync();

        // Assert

        Assert.Equal(0, result);
    }

    [Theory, AutoNSubstituteData]
    internal async Task BeginTransactionAsync_ShouldReturnTransactionGivenNoPendingTransaction(
        DbContextOptionsBuilder<TestDbContext> optionsBuilder,
        [Substitute, Frozen] IMediator mediator)
    {
        // Arrange

        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
        TestDbContext context = new(optionsBuilder.Options, mediator);
        context.Database.EnsureCreated();

        // Act

        IDbContextTransaction? result = await context.BeginTransactionAsync();

        // Assert

        Assert.NotNull(result);
    }

    [Theory, AutoNSubstituteData]
    internal async Task BeginTransactionAsync_ShouldReturnNullGivenPendingTransaction(
        DbContextOptionsBuilder<TestDbContext> optionsBuilder,
        [Substitute, Frozen] IMediator mediator)
    {
        // Arrange

        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
        TestDbContext context = new(optionsBuilder.Options, mediator);
        context.Database.EnsureCreated();

        await context.BeginTransactionAsync();

        // Act

        IDbContextTransaction? result = await context.BeginTransactionAsync();

        // Assert

        Assert.Null(result);
    }

    [Theory, AutoNSubstituteData]
    internal async Task CommitTransactionAsync_ShouldCommitGivenNoExceptions(
        DbContextOptionsBuilder<TestDbContext> optionsBuilder,
        [Substitute, Frozen] IMediator mediator)
    {
        // Arrange

        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
        TestDbContext context = new(optionsBuilder.Options, mediator);
        context.Database.EnsureCreated();

        IDbContextTransaction? transaction = await context.BeginTransactionAsync();

        // Act

        await context.CommitTransactionAsync(transaction!);

        // Assert

        Assert.Null(context.GetCurrentTransaction());
    }

    [Theory, AutoNSubstituteData]
    internal async Task CommitTransactionAsync_ShouldRollbackGivenException(
        DbContextOptionsBuilder<TestDbContext> optionsBuilder,
        IMediator mediator)
    {
        // Arrange

        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
        TestDbContext context = new(optionsBuilder.Options, mediator);
        context.Database.EnsureCreated();
        context.ThrowExceptionOnSaveChanges = true;

        IDbContextTransaction? transaction = await context.BeginTransactionAsync();

        // Act

        async Task act() => await context.CommitTransactionAsync(transaction!);

        // Assert

        await Assert.ThrowsAsync<Exception>(act);
        Assert.Null(context.GetCurrentTransaction());
    }

    [Theory, AutoNSubstituteData]
    internal async Task CommitTransactionAsync_ShouldThrowArgumentNullExceptionGivenNullTransaction(
        DbContextOptionsBuilder<TestDbContext> optionsBuilder,
        IMediator mediator)
    {
        // Arrange

        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
        TestDbContext context = new(optionsBuilder.Options, mediator);
        context.Database.EnsureCreated();

        // Act

        async Task act() => await context.CommitTransactionAsync(null);

        // Assert

        await Assert.ThrowsAsync<ArgumentNullException>(act);
        Assert.Null(context.GetCurrentTransaction());
    }

    [Theory, AutoNSubstituteData]
    internal async Task CommitTransactionAsync_ShouldThrowInvalidOperationExceptionGivenTransactionIsNotCurrent(
        DbContextOptionsBuilder<TestDbContext> optionsBuilder,
        IMediator mediator)
    {
        // Arrange

        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
        TestDbContext context = new(optionsBuilder.Options, mediator);
        context.Database.EnsureCreated();

        IDbContextTransaction? transaction = await context.BeginTransactionAsync();
        await context.CommitTransactionAsync(transaction);

        // Act

        async Task act() => await context.CommitTransactionAsync(transaction);

        // Assert

        await Assert.ThrowsAsync<InvalidOperationException>(act);
        Assert.Null(context.GetCurrentTransaction());
    }
}
