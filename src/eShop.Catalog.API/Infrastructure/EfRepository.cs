using Ardalis.Specification.EntityFrameworkCore;
using eShop.Shared.Data;

namespace eShop.Catalog.API.Infrastructure;

/// <summary>
/// The repository implementation for Entity Framework Core.
/// </summary>
/// <typeparam name="T">The entity type for the repository.</typeparam>
/// <param name="dbContext"><see cref="DbContext"/>.</param>
public class EfRepository<T>(CatalogContext dbContext) : RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    private readonly CatalogContext _dbContext = dbContext;

    public CatalogContext DbContext => this._dbContext;

    public async Task ExecuteInTransactionAsync(Func<Guid, Task> func, CancellationToken cancellationToken = default)
    {
        await ResilientTransaction.New(this._dbContext).ExecuteAsync(func);
    }
}
