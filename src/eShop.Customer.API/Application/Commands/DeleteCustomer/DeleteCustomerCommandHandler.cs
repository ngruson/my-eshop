using Ardalis.GuardClauses;
using Ardalis.Result;
using eShop.Customer.API.Application.GuardClauses;
using eShop.Customer.API.Application.Specifications;
using eShop.Shared.Data;

namespace eShop.Customer.API.Application.Commands.DeleteCustomer;

internal class DeleteCustomerCommandHandler(
    ILogger<DeleteCustomerCommandHandler> logger,
    IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> repository) : IRequestHandler<DeleteCustomerCommand, Result>
{
    private readonly ILogger<DeleteCustomerCommandHandler> logger = logger;
    private readonly IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository = repository;

    public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Soft deleting customer...");

            Domain.AggregatesModel.CustomerAggregate.Customer? customer =
                await this.customerRepository.FirstOrDefaultAsync(
                    new GetCustomerByObjectIdSpecification(request.ObjectId),
                    cancellationToken);

            var foundResult = Guard.Against.CustomerNull(customer, this.logger);
            if (!foundResult.IsSuccess)
            {
                return foundResult;
            }

            customer!.IsDeleted = true;
            customer.DeletedAtUtc = DateTime.UtcNow;

            await this.customerRepository.UpdateAsync(customer!, cancellationToken);

            this.logger.LogInformation("Customer soft deleted");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to soft delete customer.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
