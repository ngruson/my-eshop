using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Ordering.API.Core.Exceptions;

namespace eShop.Ordering.API.Core;

internal static class GuardClauses
{
    internal static Result OrdersNullOrEmpty(this IGuardClause guardClause, List<Domain.AggregatesModel.OrderAggregate.Order> input, ILogger logger)
    {
        if (input is null || input.Count == 0)
        {
            var ex = new OrdersNotFoundException();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }
}
