using eShop.Customer.Contracts.GetCustomer;

namespace eShop.ServiceInvocation.CustomerService;

public interface ICustomerService
{
    Task<CustomerDto> GetCustomer(string name);
}
