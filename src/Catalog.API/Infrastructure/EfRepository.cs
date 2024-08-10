using Ardalis.Specification.EntityFrameworkCore;
using eShop.Shared.Data;

namespace Catalog.API.Infrastructure;

/// <summary>
/// The repository implementation for Entity Framework Core.
/// </summary>
/// <typeparam name="T">The entity type for the repository.</typeparam>
/// <param name="dbContext"><see cref="DbContext"/>.</param>
public class EfRepository<T>(CatalogContext dbContext) : RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    private readonly IUnitOfWork _unitOfWork = dbContext;

    public IUnitOfWork UnitOfWork => this._unitOfWork;
}
