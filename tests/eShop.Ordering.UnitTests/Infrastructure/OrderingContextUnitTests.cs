using eShop.Ordering.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace eShop.Ordering.UnitTests.Infrastructure;

public class OrderingContextUnitTests
{
    [Theory, AutoNSubstituteData]
    public void OnModelCreating_ShouldApplyConfigurations(
        DbContextOptionsBuilder<OrderingContext> optionsBuilder,
        IMediator mediator)
    {
        // Arrange

        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");

        OrderingContext context = new(optionsBuilder.Options, mediator);

        context.Database.EnsureCreated();

        // Act

        // Assert

        Assert.True(context.Model.GetEntityTypes().Any());
    }
}
