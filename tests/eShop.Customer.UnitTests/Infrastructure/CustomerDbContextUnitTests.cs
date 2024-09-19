using eShop.Customer.Infrastructure.EFCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace eShop.Customer.UnitTests.Infrastructure;

public class CustomerDbContextUnitTests
{
    [Theory, AutoNSubstituteData]
    public void OnModelCreating_ShouldApplyConfigurations(
        DbContextOptionsBuilder<CustomerDbContext> optionsBuilder,
        IMediator mediator)
    {
        // Arrange

        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
        optionsBuilder.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

        CustomerDbContext context = new(optionsBuilder.Options, mediator);

        context.Database.EnsureCreated();

        // Act

        // Assert

        Assert.True(context.Model.GetEntityTypes().Any());
    }
}
