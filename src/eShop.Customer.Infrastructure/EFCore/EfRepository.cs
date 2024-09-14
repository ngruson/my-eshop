using Ardalis.Specification.EntityFrameworkCore;
using eShop.IntegrationEventLogEF.Utilities;
using eShop.Shared.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace eShop.Customer.Infrastructure.EFCore;

public class EfRepository<T>(CustomerDbContext dbContext) : RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    private readonly CustomerDbContext _dbContext = dbContext;

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
