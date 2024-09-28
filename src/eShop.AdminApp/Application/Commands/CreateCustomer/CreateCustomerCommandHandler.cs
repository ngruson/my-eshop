using Ardalis.Result;
using eShop.Customer.Contracts;
using MediatR;

namespace eShop.AdminApp.Application.Commands.CreateCustomer;

internal class CreateCustomerCommandHandler(
    ILogger<CreateCustomerCommandHandler> logger,
    ICustomerApi customerApi) : IRequestHandler<CreateCustomerCommand, Result>
{
    private readonly ILogger<CreateCustomerCommandHandler> logger = logger;
    private readonly ICustomerApi customerApi = customerApi;

    public async Task<Result> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Creating customer...");

            await this.customerApi.CreateCustomer(request.Dto);

            this.logger.LogInformation("Customer created");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to create customer";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
