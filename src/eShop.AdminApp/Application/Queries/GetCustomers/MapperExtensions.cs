using eShop.Customer.Contracts.GetCustomers;

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
                _.ZipCode))
            .ToList();
    }
}
