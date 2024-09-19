using eShop.Catalog.API.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace eShop.Catalog.UnitTests.Infrastructure;

public class CatalogContextUnitTests
{
    [Theory, AutoNSubstituteData]
    public void OnModelCreating_ShouldApplyConfigurations(
        DbContextOptionsBuilder<CatalogContext> optionsBuilder,
        IMediator mediator)
    {
        // Arrange

        optionsBuilder.UseInMemoryDatabase(databaseName: "testDatabase");
        optionsBuilder.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

        CatalogContext context = new(optionsBuilder.Options, mediator);

        context.Database.EnsureCreated();

        // Act

        // Assert

        Assert.True(context.Model.GetEntityTypes().Any());
    }
}
