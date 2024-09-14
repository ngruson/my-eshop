using eShop.Shared.Data.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace eShop.Shared.UnitTests.Data;

internal class TestDbContext(DbContextOptions<TestDbContext> options, IMediator mediator)
    : eShopDbContext(options, mediator)
{

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (this.ThrowExceptionOnSaveChanges)
        {
            throw new Exception("SaveChangesAsync failed");
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<TestEntity> TestEntities { get; set; }

    public bool ThrowExceptionOnSaveChanges { get; set; }
}
