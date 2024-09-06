using Ardalis.Specification;

namespace eShop.Shared.Data;

/// <summary>
/// The repository interface.
/// </summary>
/// <typeparam name="T">The entity type for the repository.</typeparam>
public interface IRepository<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot
{
    //IUnitOfWork UnitOfWork { get; }

    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);

    Task ExecuteInTransactionAsync(Func<Guid, Task> func, CancellationToken cancellationToken = default);
}
