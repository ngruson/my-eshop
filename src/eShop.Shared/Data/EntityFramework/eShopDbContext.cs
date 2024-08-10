using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;
using System.Data;

namespace eShop.Shared.Data.EntityFramework;
public class eShopDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;
    private IDbContextTransaction? _currentTransaction;

    //public eShopDbContext(DbContextOptions<eShopDbContext> options) : base(options) { }

    public IDbContextTransaction? GetCurrentTransaction() => this._currentTransaction;

    public bool HasActiveTransaction => this._currentTransaction != null;

    public eShopDbContext(DbContextOptions options, IMediator mediator) : base(options)
    {
        this._mediator = mediator;

        System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.ConfigureSmartEnum();
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        await this._mediator.DispatchDomainEventsAsync(this);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed
        _ = await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (this._currentTransaction != null) return null;

        this._currentTransaction = await this.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return this._currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != this._currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await this.SaveChangesAsync();
            await transaction.CommitAsync();
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
