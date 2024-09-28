using Refit;

namespace eShop.Customer.Contracts;

public interface ICustomerApi
{
    [Get("/api/customers/all?api-version=1.0")]
    Task<GetCustomers.CustomerDto[]> GetCustomers();

    [Get("/api/customers?firstName={firstName}&lastName={lastName}&api-version=1.0")]
    Task<GetCustomer.CustomerDto> GetCustomer(string firstName, string lastName);

    [Post("/api/customers?api-version=1.0")]
    Task CreateCustomer(CreateCustomer.CreateCustomerDto dto);

    [Put("/api/customers?api-version=1.0")]
    Task UpdateCustomer(UpdateCustomer.UpdateCustomerDto dto);
}
