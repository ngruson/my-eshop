using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Ordering.API.Application.Exceptions;

namespace eShop.Ordering.API.Application;

internal static class GuardClauses
{
    internal static Result OrdersNullOrEmpty(this IGuardClause guardClause, List<Domain.AggregatesModel.OrderAggregate.Order> input, ILogger logger)
    {
        if (input is null || input.Count == 0)
        {
            OrdersNotFoundException ex = new();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }
}
