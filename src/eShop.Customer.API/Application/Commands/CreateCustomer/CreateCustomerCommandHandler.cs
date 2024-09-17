using Ardalis.Result;
using eShop.Shared.Data;

namespace eShop.Customer.API.Application.Commands.CreateCustomer;

internal class CreateCustomerCommandHandler(
    ILogger<CreateCustomerCommandHandler> logger,
    IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> repository) : IRequestHandler<CreateCustomerCommand, Result>
{
    private readonly ILogger<CreateCustomerCommandHandler> logger = logger;
    private readonly IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository = repository;

    public async Task<Result> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Creating customer...");

            Domain.AggregatesModel.CustomerAggregate.Customer customer = request.Dto.MapFromDto();

            await this.customerRepository.AddAsync(customer, cancellationToken);

            this.logger.LogInformation("Customer created");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to create customer.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
