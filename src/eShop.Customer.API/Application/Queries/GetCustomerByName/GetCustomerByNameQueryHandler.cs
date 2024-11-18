using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Customer.API.Application.GuardClauses;
using eShop.Customer.API.Application.Queries.GetCustomerByObjectId;
using eShop.Customer.API.Application.Specifications;
using eShop.Customer.Contracts.GetCustomer;
using eShop.Shared.Data;

namespace eShop.Customer.API.Application.Queries.GetCustomerByName;

public class GetCustomerByNameQueryHandler(
    ILogger<GetCustomerByNameQueryHandler> logger,
    IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository) : IRequestHandler<GetCustomerByNameQuery, Result<CustomerDto>>
{
    private readonly ILogger<GetCustomerByNameQueryHandler> logger = logger;
    private readonly IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository = customerRepository;

    public async Task<Result<CustomerDto>> Handle(GetCustomerByNameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving customer by name...");

            Domain.AggregatesModel.CustomerAggregate.Customer? customer =
                await this.customerRepository.FirstOrDefaultAsync(
                    new GetCustomerByNameSpecification(request.Name),
                    cancellationToken);

            Result foundResult = Guard.Against.CustomerNull(customer, this.logger);
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
