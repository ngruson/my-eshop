using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Invoicing.API.Application.Exceptions;
using eShop.Ordering.Contracts.GetOrder;

namespace eShop.Invoicing.API.Application.GuardClauses;

internal static class GuardClauses
{
    public static Result OrderNull(this IGuardClause guardClause, OrderDto? input, ILogger logger)
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
