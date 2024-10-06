using eShop.AdminApp.Application.Queries.MasterData.GetCountries;
using eShop.AdminApp.Application.Queries.MasterData.GetStates;
using eShop.Customer.Contracts.GetCustomer;

namespace eShop.AdminApp.Application.Queries.Customer.GetCustomer;

internal static class MapperExtensions
{
    internal static CustomerViewModel MapToCustomerViewModel(this CustomerDto customer,
        CountryViewModel[] countries, StateViewModel[] states)
    {
        CountryViewModel country = countries.First(countries => countries.Code == customer.Country);
        StateViewModel state = states.First(states => states.Code == customer.State);

        return new CustomerViewModel(
            customer.ObjectId,
            customer.UserName,
            customer.FirstName,
            customer.LastName,
            customer.Street,
            customer.City,
            state.Name,
            country.Name,
            customer.ZipCode,
            customer.CardNumber,
            customer.Expiration,
            customer.CardHolderName,
            customer.CardType);
    }
}
