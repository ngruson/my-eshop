using Ardalis.Result;
using eShop.ServiceInvocation.CustomerApiClient;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Customer.DeleteCustomer;

internal class DeleteCustomerCommandHandler(
    ILogger<DeleteCustomerCommandHandler> logger,
    ICustomerApiClient customerApiClient) : IRequestHandler<DeleteCustomerCommand, Result>
{
    private readonly ILogger<DeleteCustomerCommandHandler> logger = logger;
    private readonly ICustomerApiClient customerApiClient = customerApiClient;

    public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Deleting customer {ObjectId}...", request.ObjectId);

            await this.customerApiClient.DeleteCustomer(request.ObjectId);

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
