using Ardalis.Result;
using eShop.ServiceInvocation.CustomerService;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Customer.CreateCustomer;

internal class CreateCustomerCommandHandler(
    ILogger<CreateCustomerCommandHandler> logger,
    ICustomerService customerService) : IRequestHandler<CreateCustomerCommand, Result>
{
    private readonly ILogger<CreateCustomerCommandHandler> logger = logger;
    private readonly ICustomerService customerService = customerService;

    public async Task<Result> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Creating customer...");

            await this.customerService.CreateCustomer(request.Dto);

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
