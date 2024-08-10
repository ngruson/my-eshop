using Ardalis.Specification.EntityFrameworkCore;
using eShop.Ordering.Infrastructure;
using eShop.Shared.Data;

namespace Ordering.Infrastructure.Repositories;
/// <summary>
/// The repository implementation for Entity Framework Core.
/// </summary>
/// <typeparam name="T">The entity type for the repository.</typeparam>
/// <param name="dbContext"><see cref="DbContext"/>.</param>
public class EfRepository<T>(OrderingContext dbContext) : RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    private readonly IUnitOfWork _unitOfWork = dbContext;

    public IUnitOfWork UnitOfWork => this._unitOfWork;
}
