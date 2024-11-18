namespace eShop.Shared.Data;

public record PaginatedItems<TEntity>(int PageIndex, int PageSize, long Count, TEntity[] Data) where TEntity : class;
