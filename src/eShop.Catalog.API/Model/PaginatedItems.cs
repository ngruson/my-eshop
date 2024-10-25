namespace eShop.Catalog.API.Model;

public record PaginatedItems<TEntity>(int PageIndex, int PageSize, long Count, TEntity[] Data) where TEntity : class;
