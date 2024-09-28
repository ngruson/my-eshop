using eShop.AdminApp.Application.Queries.GetCustomers;
using eShop.MasterData.Contracts;

namespace eShop.AdminApp.Components.Pages.Customers;

public record CustomerDialogContent(
    CustomerViewModel Model,
    IEnumerable<CountryDto> Countries,
    IEnumerable<StateDto> States);
