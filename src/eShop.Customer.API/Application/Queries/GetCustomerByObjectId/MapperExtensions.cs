using eShop.Customer.Contracts.GetCustomer;

namespace eShop.Customer.API.Application.Queries.GetCustomerByObjectId;

internal static class MapperExtensions
{
    public static CustomerDto MapToCustomerDto(this Domain.AggregatesModel.CustomerAggregate.Customer customer)
    {
        return new CustomerDto(
            customer.ObjectId,
            customer.UserName!,
            customer.FirstName!,
            customer.LastName!,
            customer.Street!,
            customer.City!,
            customer.State!,
            customer.Country!,
            customer.ZipCode!,
            customer.CardNumber?[^4..].PadLeft(customer.CardNumber.Length, 'X'),
            customer.Expiration,
            customer.CardHolderName,
            customer.CardType?.Name);
    }
}
