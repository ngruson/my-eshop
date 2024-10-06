using Refit;

namespace eShop.Customer.Contracts;

public interface ICustomerApi
{
    [Get("/api/customers/all?api-version=1.0")]
    Task<GetCustomers.CustomerDto[]> GetCustomers();

    [Get("/api/customers/{objectId}?api-version=1.0")]
    Task<GetCustomer.CustomerDto> GetCustomer(Guid objectId);

    [Get("/api/customers/name/{name}?api-version=1.0")]
    Task<GetCustomer.CustomerDto> GetCustomer(string name);

    [Post("/api/customers?api-version=1.0")]
    Task CreateCustomer(CreateCustomer.CreateCustomerDto dto);

    [Put("/api/customers?api-version=1.0")]
    Task UpdateCustomer(UpdateCustomer.UpdateCustomerDto dto);
}
