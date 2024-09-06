using Ardalis.Specification;

namespace eShop.Shared.Data;
/// <summary>
/// The repository interface for reads.
/// </summary>
/// <typeparam name="T">The entity type for the repository.</typeparam>
public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class, IAggregateRoot
{
}
