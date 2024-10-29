using Ardalis.Result;
using eShop.ServiceInvocation.CustomerService;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Customer.UpdateCustomer;

internal class UpdateCustomerCommandHandler(
    ILogger<UpdateCustomerCommandHandler> logger,
    ICustomerService customerService) : IRequestHandler<UpdateCustomerCommand, Result>
{
    private readonly ILogger<UpdateCustomerCommandHandler> logger = logger;
    private readonly ICustomerService customerService = customerService;

    public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Updating customer...");

            await this.customerService.UpdateCustomer(request.ObjectId, request.Dto);

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
