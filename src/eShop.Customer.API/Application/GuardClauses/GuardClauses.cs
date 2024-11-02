using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Customer.API.Application.Exceptions;

namespace eShop.Customer.API.Application.GuardClauses;

internal static class GuardClauses
{
    internal static Result CustomersNullOrEmpty(this IGuardClause guardClause, List<Domain.AggregatesModel.CustomerAggregate.Customer> input, ILogger logger)
    {
        if (input is null || input.Count == 0)
        {
            CustomersNotFoundException ex = new();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }

    internal static Result CustomerNull(this IGuardClause guardClause, Domain.AggregatesModel.CustomerAggregate.Customer? input, ILogger logger)
    {
        if (input is null)
        {
            CustomerNotFoundException ex = new();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }
}
