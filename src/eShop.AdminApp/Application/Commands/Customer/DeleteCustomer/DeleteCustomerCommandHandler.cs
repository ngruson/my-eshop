using Ardalis.Result;
using eShop.Customer.Contracts;
using MediatR;

namespace eShop.AdminApp.Application.Commands.Customer.DeleteCustomer;

internal class DeleteCustomerCommandHandler(
    ILogger<DeleteCustomerCommandHandler> logger,
    ICustomerApi customerApi) : IRequestHandler<DeleteCustomerCommand, Result>
{
    private readonly ILogger<DeleteCustomerCommandHandler> logger = logger;
    private readonly ICustomerApi customerApi = customerApi;

    public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Deleting customer {ObjectId}...", request.ObjectId);

            await this.customerApi.DeleteCustomer(request.ObjectId);

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
