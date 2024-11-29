using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Ordering.API.Application.Exceptions;

namespace eShop.Ordering.API.Application.GuardClauses;

internal static class GuardClauses
{
    public static Result OrderNull(this IGuardClause guardClause, Order? input, ILogger logger)
    {
        if (input is null)
        {
            OrderNotFoundException ex = new();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }
}
