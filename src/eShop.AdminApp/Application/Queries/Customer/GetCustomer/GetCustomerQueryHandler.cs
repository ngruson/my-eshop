using Ardalis.Result;
using eShop.Customer.Contracts.GetCustomer;
using eShop.ServiceInvocation.CustomerApiClient;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Customer.GetCustomer;

public class GetCustomerQueryHandler(
    ILogger<GetCustomerQueryHandler> logger,
    ICustomerApiClient customerApiClient) : IRequestHandler<GetCustomerQuery, Result<CustomerViewModel>>
{
    private readonly ILogger<GetCustomerQueryHandler> logger = logger;
    private readonly ICustomerApiClient customerApiClient = customerApiClient;

    public async Task<Result<CustomerViewModel>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving customer {ObjectId} from API...", request.ObjectId);

            CustomerDto customer = await this.customerApiClient.GetCustomer(request.ObjectId);

            this.logger.LogInformation("Retrieved customer {ObjectId} from API", request.ObjectId);

           return customer
                .MapToCustomerViewModel();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve customer.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
