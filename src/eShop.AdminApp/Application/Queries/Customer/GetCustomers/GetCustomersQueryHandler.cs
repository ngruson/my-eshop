using Ardalis.Result;
using eShop.AdminApp.Application.Queries.MasterData.GetCountries;
using eShop.AdminApp.Application.Queries.MasterData.GetStates;
using eShop.Customer.Contracts;
using eShop.Customer.Contracts.GetCustomers;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Customer.GetCustomers;

public class GetCustomersQueryHandler(
    ILogger<GetCustomersQueryHandler> logger,
    ICustomerApi customerApi,
    IMediator mediator) : IRequestHandler<GetCustomersQuery, Result<List<CustomerViewModel>>>
{
    private readonly ILogger<GetCustomersQueryHandler> logger = logger;
    private readonly ICustomerApi customerApi = customerApi;
    private readonly IMediator mediator = mediator;

    public async Task<Result<List<CustomerViewModel>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            CustomerDto[] customers = await this.customerApi.GetCustomers();

            Result<CountryViewModel[]> countries = await this.mediator.Send(new GetCountriesQuery(), cancellationToken);
            Result<StateViewModel[]> states = await this.mediator.Send(new GetStatesQuery(), cancellationToken);

            return customers
                .ToList()
                .MapToCustomerViewModelList(countries.Value, states.Value);
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve customers.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
