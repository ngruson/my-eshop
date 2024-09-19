using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;
using System.Data;

namespace eShop.Shared.Data.EntityFramework;
public abstract class eShopDbContext : DbContext, IUnitOfWork<IDbContextTransaction>
{
    private readonly IMediator _mediator;
    private IDbContextTransaction? _currentTransaction;

    //public eShopDbContext(DbContextOptions<eShopDbContext> options) : base(options) { }

    public IDbContextTransaction? GetCurrentTransaction() => this._currentTransaction;

    public bool HasActiveTransaction => this._currentTransaction != null;

    public eShopDbContext(DbContextOptions options, IMediator mediator) : base(options)
    {
        this._mediator = mediator;

        System.Diagnostics.Debug.WriteLine("eShopDbContext::ctor ->" + this.GetHashCode());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.ConfigureSmartEnum();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers.
        await this._mediator.DispatchDomainEventsAsync(this);

        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (this._currentTransaction != null) return null;

        this._currentTransaction = await this.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return this._currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction? transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        if (transaction != this._currentTransaction)
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await this.SaveChangesAsync();
            await this._currentTransaction.CommitAsync();
        }
        catch
        {
            this.RollbackTransaction();
            throw;
        }
        finally
        {
            if (this.HasActiveTransaction)
            {
                this._currentTransaction.Dispose();
                this._currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            this._currentTransaction?.Rollback();
        }
        finally
        {
            if (this.HasActiveTransaction)
            {
                this._currentTransaction?.Dispose();
                this._currentTransaction = null;
            }
        }
    }
}
