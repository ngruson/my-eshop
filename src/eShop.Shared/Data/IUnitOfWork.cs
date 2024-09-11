namespace eShop.Shared.Data;

public interface IUnitOfWork<T> : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<T?> BeginTransactionAsync();
    Task CommitTransactionAsync(T transaction);
}
