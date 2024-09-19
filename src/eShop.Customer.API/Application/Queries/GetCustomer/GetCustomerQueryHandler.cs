using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Customer.API.Application.GuardClauses;
using eShop.Customer.API.Application.Specifications;
using eShop.Customer.Contracts.GetCustomer;
using eShop.Shared.Data;

namespace eShop.Customer.API.Application.Queries.GetCustomer;

internal class GetCustomerQueryHandler(
    ILogger<GetCustomerQueryHandler> logger,
    IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository)
        : IRequestHandler<GetCustomerQuery, Result<CustomerDto>>
{
    private readonly ILogger<GetCustomerQueryHandler> logger = logger;
    private readonly IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository = customerRepository;

    public async Task<Result<CustomerDto>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving customer...");

            Domain.AggregatesModel.CustomerAggregate.Customer? customer =
                await this.customerRepository.FirstOrDefaultAsync(
                    new GetCustomerSpecification(request.FirstName, request.LastName),
                    cancellationToken);

            var foundResult = Guard.Against.CustomerNull(customer, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Returning customer.");

            return customer!.MapToCustomerDto();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve customer.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
