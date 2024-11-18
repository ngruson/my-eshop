namespace eShop.IntegrationEventLogEF.Utilities;

public class ResilientTransaction
{
    private readonly DbContext _context;
    private ResilientTransaction(DbContext context) =>
        this._context = context ?? throw new ArgumentNullException(nameof(context));

    public static ResilientTransaction New(DbContext context) => new(context);

    public async Task ExecuteAsync(Func<Guid, Task> func)
    {
        //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
        IExecutionStrategy strategy = this._context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using IDbContextTransaction transaction = await this._context.Database.BeginTransactionAsync();
            await func(transaction.TransactionId);
            await transaction.CommitAsync();
        });
    }
}
