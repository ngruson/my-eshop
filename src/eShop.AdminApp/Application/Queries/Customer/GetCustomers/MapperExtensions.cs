using eShop.AdminApp.Application.Commands.Customer.CreateCustomer;
using eShop.AdminApp.Application.Queries.MasterData.GetCountries;
using eShop.AdminApp.Application.Queries.MasterData.GetStates;
using eShop.Customer.Contracts.GetCustomers;

namespace eShop.AdminApp.Application.Queries.Customer.GetCustomers;

internal static class MapperExtensions
{
    internal static List<CustomerViewModel> MapToCustomerViewModelList(this List<CustomerDto> customers,
        CountryViewModel[] countries, StateViewModel[] states)
    {
        return customers
            .Select(_ => new CustomerViewModel(
                _.ObjectId,
                _.UserName,
                _.FirstName,
                _.LastName,
                _.Street,
                _.City,
                GetStateName(states, _.State),
                GetCountryName(countries, _.Country),
                _.ZipCode,
                _.CardNumber,
                _.SecurityNumber,
                _.Expiration,
                _.CardHolderName,
                _.CardType,
                _.IsDeleted))
            .ToList();
    }

    private static string GetStateName(StateViewModel[] states, string code)
    {
        return states.FirstOrDefault(state => state.Code == code)?.Name ?? string.Empty;
    }

    private static string GetCountryName(CountryViewModel[] countries, string code)
    {
        return countries.FirstOrDefault(country => country.Code == code)?.Name ?? string.Empty;
    }

    internal static CreateCustomerCommand MapToCreateCustomerCreateCommand(this CustomerViewModel model)
    {
        return new CreateCustomerCommand(
            new eShop.Customer.Contracts.CreateCustomer.CreateCustomerDto(
                model.UserName,
                model.FirstName,
                model.LastName,
                model.Street,
                model.City,
                model.State!,
                model.Country,
                model.ZipCode,
                model.CardNumber,
                model.SecurityNumber,
                model.Expiration,
                model.CardHolderName,
                model.CardType));
    }
}
