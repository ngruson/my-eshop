using Ardalis.Result;
using eShop.AdminApp.Application.Queries.GetCustomers;
using eShop.Customer.Contracts;
using eShop.Customer.Contracts.GetCustomers;
using eShop.Ordering.Contracts.GetOrders;
using MediatR;

namespace eShop.AdminApp.Application.Queries.GetCustomers;

public class GetCustomersQueryHandler(
    ILogger<GetCustomersQueryHandler> logger,
    ICustomerApi customerApi) : IRequestHandler<GetCustomersQuery, Result<List<CustomerViewModel>>>
{
    private readonly ILogger<GetCustomersQueryHandler> logger = logger;
    private readonly ICustomerApi customerApi = customerApi;

    public async Task<Result<List<CustomerViewModel>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            CustomerDto[] customers = await this.customerApi.GetCustomers();

            return customers
                .ToList()
                .MapToCustomerViewModelList();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve customers.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
