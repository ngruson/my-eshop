using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Catalog.API.Application.Exceptions;

namespace eShop.Catalog.API.Application.GuardClauses;

internal static class GuardClauses
{
    internal static Result CatalogItemsNullOrEmpty(this IGuardClause guardClause, List<CatalogItem> input, ILogger logger)
    {
        if (input is null || input.Count == 0)
        {
            CatalogItemsNotFoundException ex = new();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }

    internal static Result CatalogTypesNullOrEmpty(this IGuardClause guardClause, List<CatalogType> input, ILogger logger)
    {
        if (input is null || input.Count == 0)
        {
            CatalogTypesNotFoundException ex = new();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }

    internal static Result CatalogBrandsNullOrEmpty(this IGuardClause guardClause, List<CatalogBrand> input, ILogger logger)
    {
        if (input is null || input.Count == 0)
        {
            CatalogBrandsNotFoundException ex = new();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }

    internal static Result CatalogItemNull(this IGuardClause guardClause, CatalogItem? input, ILogger logger)
    {
        if (input is null)
        {
            CatalogItemNotFoundException ex = new();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }

    internal static Result CatalogTypeNull(this IGuardClause guardClause, CatalogType? input, ILogger logger)
    {
        if (input is null)
        {
            CatalogTypeNotFoundException ex = new();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }

    internal static Result CatalogBrandNull(this IGuardClause guardClause, CatalogBrand? input, ILogger logger)
    {
        if (input is null)
        {
            CatalogBrandNotFoundException ex = new();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }
}
