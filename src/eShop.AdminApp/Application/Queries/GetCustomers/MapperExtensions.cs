using eShop.AdminApp.Application.Commands.CreateCustomer;
using eShop.AdminApp.Application.Commands.UpdateCustomer;
using eShop.Customer.Contracts.GetCustomers;
using eShop.MasterData.Contracts;

namespace eShop.AdminApp.Application.Queries.GetCustomers;

internal static class MapperExtensions
{
    internal static List<CustomerViewModel> MapToCustomerViewModelList(this List<CustomerDto> customers)
    {
        return customers
            .Select(_ => new CustomerViewModel(
                _.FirstName,
                _.LastName,
                _.Street,
                _.City,
                _.State,
                _.Country,
                _.ZipCode,
                _.CardNumber,
                _.SecurityNumber,
                _.Expiration,
                _.CardHolderName,
                _.CardType))
            .ToList();
    }

    internal static CreateCustomerCommand MapToCreateCustomerCreateCommand(this CustomerViewModel model)
    {
        return new CreateCustomerCommand(
            new Customer.Contracts.CreateCustomer.CreateCustomerDto(
                model.FirstName,
                model.LastName,
                model.Street,
                model.City,
                model.State,
                model.Country,
                model.ZipCode,
                model.CardNumber,
                model.SecurityNumber,
                model.Expiration,
                model.CardHolderName,
                model.CardType));
    }

    internal static UpdateCustomerCommand MapToUpdateCustomerCreateCommand(this CustomerViewModel model)
    {
        return new UpdateCustomerCommand(
            new Customer.Contracts.UpdateCustomer.UpdateCustomerDto(
                model.FirstName,
                model.LastName,
                model.Street,
                model.City,
                model.State,
                model.Country,
                model.ZipCode,
                model.CardNumber,
                model.SecurityNumber,
                model.Expiration,
                model.CardHolderName,
                model.CardType));
    }
}
