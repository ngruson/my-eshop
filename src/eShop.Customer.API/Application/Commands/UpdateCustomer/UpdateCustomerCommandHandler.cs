using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Customer.API.Application.GuardClauses;
using eShop.Customer.API.Application.Specifications;
using eShop.Shared.Data;

namespace eShop.Customer.API.Application.Commands.UpdateCustomer;

internal class UpdateCustomerCommandHandler(
    ILogger<UpdateCustomerCommandHandler> logger,
    IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> repository) : IRequestHandler<UpdateCustomerCommand, Result>
{
    private readonly ILogger<UpdateCustomerCommandHandler> logger = logger;
    private readonly IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository = repository;

    public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Updating customer...");

            Domain.AggregatesModel.CustomerAggregate.Customer? customer =
                await this.customerRepository.FirstOrDefaultAsync(
                    new GetCustomerSpecification(request.Dto.FirstName, request.Dto.LastName),
                    cancellationToken);

            // Log customer first name and last name
            this.logger.LogInformation("Customer first name: {FirstName}, last name: {LastName}", request.Dto.FirstName, request.Dto.LastName);

            var foundResult = Guard.Against.CustomerNull(customer, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            request.Dto.MapFromDto(customer!);

            await this.customerRepository.UpdateAsync(customer!, cancellationToken);

            this.logger.LogInformation("Customer updated");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to update customer.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
