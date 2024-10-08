using Ardalis.Result;
using eShop.AdminApp.Application.Queries.MasterData.GetCountries;
using eShop.AdminApp.Application.Queries.MasterData.GetStates;
using eShop.Customer.Contracts;
using eShop.Customer.Contracts.GetCustomer;
using MediatR;

namespace eShop.AdminApp.Application.Queries.Customer.GetCustomer;

public class GetCustomerQueryHandler(
    ILogger<GetCustomerQueryHandler> logger,
    ICustomerApi customerApi,
    IMediator mediator) : IRequestHandler<GetCustomerQuery, Result<CustomerViewModel>>
{
    private readonly ILogger<GetCustomerQueryHandler> logger = logger;
    private readonly ICustomerApi customerApi = customerApi;
    private readonly IMediator mediator = mediator;

    public async Task<Result<CustomerViewModel>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving customer {ObjectId} from API...", request.ObjectId);

            CustomerDto customer = await this.customerApi.GetCustomer(request.ObjectId);

            this.logger.LogInformation("Retrieved customer {ObjectId} from API", request.ObjectId);

            //Result<CountryViewModel[]> countries = await this.mediator.Send(new GetCountriesQuery(), cancellationToken);
            //Result<StateViewModel[]> states = await this.mediator.Send(new GetStatesQuery(), cancellationToken);

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
