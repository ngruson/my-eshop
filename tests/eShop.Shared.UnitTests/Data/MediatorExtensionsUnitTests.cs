using eShop.Shared.Data.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace eShop.Shared.UnitTests.Data;

public class MediatorExtensionsUnitTests
{
    private static TestDbContext GetDbContext()
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
    internal async Task test(
        IMediator mediator
        )
    {
        // Arrange

        TestDbContext testDbContext = GetDbContext();

        // Act

        await mediator.DispatchDomainEventsAsync(testDbContext);

        // Assert
    }
}
