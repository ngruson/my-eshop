using eShop.Shared.Data.EntityFramework;
using eShop.Shared.UnitTests.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace eShop.Shared.UnitTests.Behaviors;

internal class TestDbContext(DbContextOptions<TestDbContext> options, IMediator mediator) : eShopDbContext(options, mediator)
{
    public DbSet<TestEntity> TestEntities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
    }
}
