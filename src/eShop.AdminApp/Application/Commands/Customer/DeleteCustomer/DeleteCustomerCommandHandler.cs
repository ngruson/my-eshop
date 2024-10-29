using Ardalis.Result;
using eShop.ServiceInvocation.CustomerService;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Customer.DeleteCustomer;

internal class DeleteCustomerCommandHandler(
    ILogger<DeleteCustomerCommandHandler> logger,
    ICustomerService customerService) : IRequestHandler<DeleteCustomerCommand, Result>
{
    private readonly ILogger<DeleteCustomerCommandHandler> logger = logger;
    private readonly ICustomerService customerService = customerService;

    public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Deleting customer {ObjectId}...", request.ObjectId);

            await this.customerService.DeleteCustomer(request.ObjectId);

            this.logger.LogInformation("Customer deleted");

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to delete customer";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
