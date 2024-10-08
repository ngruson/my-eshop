using eShop.AdminApp.Application.Commands.Customer.CreateCustomer;
using eShop.AdminApp.Application.Commands.Customer.UpdateCustomer;
using eShop.Customer.Contracts.CreateCustomer;
using eShop.Customer.Contracts.GetCustomer;
using eShop.Customer.Contracts.UpdateCustomerGeneralInfo;

namespace eShop.AdminApp.Application.Queries.Customer.GetCustomer;

internal static class MapperExtensions
{
    internal static CustomerViewModel MapToCustomerViewModel(this CustomerDto customer)
    {
        return new CustomerViewModel(
            customer.ObjectId,
            customer.UserName,
            customer.FirstName,
            customer.LastName,
            customer.Street,
            customer.City,
            customer.State,
            customer.Country,
            customer.ZipCode,
            customer.CardNumber,
            customer.Expiration,
            customer.CardHolderName,
            customer.CardType);
    }

    internal static CreateCustomerCommand MapToCreateCustomerCommand(this CustomerViewModel customer)
    {
        return new CreateCustomerCommand(
            new CreateCustomerDto(
                customer.UserName,
                customer.FirstName,
                customer.LastName,
                customer.Street,
                customer.City,
                customer.State,
                customer.Country,
                customer.ZipCode,
                null,
                null,
                null,
                null,
                null));
    }

    internal static UpdateCustomerCommand MapToUpdateCustomerCommand(this CustomerViewModel customer)
    {
        return new UpdateCustomerCommand(
            customer.ObjectId,
            new UpdateCustomerDto(
                customer.UserName,
                customer.FirstName,
                customer.LastName,
                customer.Street,
                customer.City,
                customer.State,
                customer.Country,
                customer.ZipCode));
    }
}
