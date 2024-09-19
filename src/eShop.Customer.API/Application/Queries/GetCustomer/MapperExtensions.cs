using eShop.Customer.Contracts.GetCustomer;

namespace eShop.Customer.API.Application.Queries.GetCustomer;

internal static class MapperExtensions
{
    public static CustomerDto MapToCustomerDto(this Domain.AggregatesModel.CustomerAggregate.Customer customer)
    {
        return new CustomerDto(
            customer.FirstName!,
            customer.LastName!,
            customer.CardNumber!,
            customer.SecurityNumber!,
            customer.Expiration!,
            customer.CardHolderName!,
            customer.CardType!,
            customer.Street!,
            customer.City!,
            customer.State!,
            customer.Country!,
            customer.ZipCode!);
    }
}
