using Ardalis.Result;
using eShop.Customer.API.Application.GuardClauses;
using eShop.Customer.Contracts.GetCustomers;
using eShop.Shared.Data;
using Ardalis.GuardClauses;
using eShop.Customer.API.Application.Specifications;

namespace eShop.Customer.API.Application.Queries.GetCustomers;

internal class GetCustomersQueryHandler(
    ILogger<GetCustomersQueryHandler> logger,
    IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository)
        : IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>
{
    private readonly ILogger<GetCustomersQueryHandler> logger = logger;
    private readonly IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository = customerRepository;

    public async Task<Result<List<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Getting customers.");

            List<Domain.AggregatesModel.CustomerAggregate.Customer> customers =
                await this.customerRepository.ListAsync(new GetCustomersSpecification(request.IncludeDeleted), cancellationToken);

            var foundResult = Guard.Against.CustomersNullOrEmpty(customers, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Retrieved {Count} customers.", customers.Count);

            return customers
                .MapToCustomerDtoList();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve customers.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
