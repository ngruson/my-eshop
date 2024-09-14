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
            var ex = new CustomersNotFoundException();
            logger.LogError(ex, "Exception: {Message}", ex.Message);
            return Result.NotFound();
        }

        return Result.Success();
    }
}
