using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Customer.API.Application.GuardClauses;
using eShop.Customer.API.Application.Specifications;
using eShop.Customer.Contracts.GetCustomer;
using eShop.Shared.Data;

namespace eShop.Customer.API.Application.Queries.GetCustomerByObjectId;

internal class GetCustomerByObjectIdQueryHandler(
    ILogger<GetCustomerByObjectIdQueryHandler> logger,
    IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository)
        : IRequestHandler<GetCustomerByObjectIdQuery, Result<CustomerDto>>
{
    private readonly ILogger<GetCustomerByObjectIdQueryHandler> logger = logger;
    private readonly IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository = customerRepository;

    public async Task<Result<CustomerDto>> Handle(GetCustomerByObjectIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving customer by object id {ObjectId}...", request.ObjectId);

            Domain.AggregatesModel.CustomerAggregate.Customer? customer =
                await this.customerRepository.FirstOrDefaultAsync(
                    new GetCustomerByObjectIdSpecification(request.ObjectId),
                    cancellationToken);

            Result foundResult = Guard.Against.CustomerNull(customer, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            this.logger.LogInformation("Retrieved customer {ObjectId}", request.ObjectId);

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
