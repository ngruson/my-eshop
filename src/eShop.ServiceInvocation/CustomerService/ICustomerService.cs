using eShop.Customer.Contracts.CreateCustomer;
using eShop.Customer.Contracts.UpdateCustomerGeneralInfo;

namespace eShop.ServiceInvocation.CustomerService;

public interface ICustomerService
{
    Task CreateCustomer(CreateCustomerDto dto);
    Task UpdateCustomer(Guid objectId, UpdateCustomerDto dto);
    Task DeleteCustomer(Guid objectId);
    Task<Customer.Contracts.GetCustomers.CustomerDto[]> GetCustomers(bool includeDeleted = false);
    Task<Customer.Contracts.GetCustomer.CustomerDto> GetCustomer(Guid objectId);
    Task<Customer.Contracts.GetCustomer.CustomerDto> GetCustomer(string name);
}
