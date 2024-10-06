using eShop.Customer.Contracts.GetCustomers;

namespace eShop.Customer.API.Application.Queries.GetCustomers;

internal static class MapperExtensions
{
    internal static List<CustomerDto> MapToCustomerDtoList(this List<Domain.AggregatesModel.CustomerAggregate.Customer> customers)
    {
        return customers
            .Select(c => new CustomerDto(
                c.ObjectId,
                c.UserName!,
                c.FirstName!,
                c.LastName!,
                c.Street!,
                c.City!,
                c.State!,
                c.Country!,
                c.ZipCode!,
                c.CardNumber,
                c.SecurityNumber,
                c.Expiration,
                c.CardHolderName,
                c.CardType?.Name))
            .ToList();
    }
}
