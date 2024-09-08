using Ardalis.Specification.EntityFrameworkCore;
using eShop.IntegrationEventLogEF.Utilities;
using eShop.Shared.Data;

namespace eShop.Ordering.Infrastructure.Repositories;
/// <summary>
/// The repository implementation for Entity Framework Core.
/// </summary>
/// <typeparam name="T">The entity type for the repository.</typeparam>
/// <param name="dbContext"><see cref="DbContext"/>.</param>
public class EfRepository<T>(OrderingContext dbContext) : RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    private readonly OrderingContext _dbContext = dbContext;

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        return await this._dbContext.SaveEntitiesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        await this._dbContext.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        IDbContextTransaction? transaction = this._dbContext.GetCurrentTransaction();

        if (transaction != null)
        {
            await this._dbContext.CommitTransactionAsync(transaction);
        }
    }

    public async Task ExecuteInTransactionAsync(Func<Guid, Task> func, CancellationToken cancellationToken = default)
    {
        await ResilientTransaction.New(this._dbContext).ExecuteAsync(func);
    }
}
