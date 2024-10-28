using eShop.Customer.Contracts;
using eShop.Customer.Contracts.GetCustomer;

namespace eShop.ServiceInvocation.CustomerService.Refit;

public class CustomerService(ICustomerApi customerApi) : ICustomerService
{
    public async Task<CustomerDto> GetCustomer(string name)
    {
        return await customerApi.GetCustomer(name);
    }
}
