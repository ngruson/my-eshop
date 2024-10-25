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
            var ex = new CatalogItemsNotFoundException();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }

    internal static Result CatalogTypesNullOrEmpty(this IGuardClause guardClause, List<CatalogType> input, ILogger logger)
    {
        if (input is null || input.Count == 0)
        {
            var ex = new CatalogTypesNotFoundException();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }

    internal static Result CatalogBrandsNullOrEmpty(this IGuardClause guardClause, List<CatalogBrand> input, ILogger logger)
    {
        if (input is null || input.Count == 0)
        {
            var ex = new CatalogBrandsNotFoundException();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }

    internal static Result CatalogItemNull(this IGuardClause guardClause, CatalogItem? input, ILogger logger)
    {
        if (input is null)
        {
            var ex = new CatalogItemNotFoundException();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }

    internal static Result CatalogTypeNull(this IGuardClause guardClause, CatalogType? input, ILogger logger)
    {
        if (input is null)
        {
            var ex = new CatalogTypeNotFoundException();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }

    internal static Result CatalogBrandNull(this IGuardClause guardClause, CatalogBrand? input, ILogger logger)
    {
        if (input is null)
        {
            var ex = new CatalogBrandNotFoundException();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }
}
