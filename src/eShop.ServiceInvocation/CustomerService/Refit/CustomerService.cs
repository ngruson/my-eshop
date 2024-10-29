using eShop.Customer.Contracts;
using eShop.Customer.Contracts.CreateCustomer;
using eShop.Customer.Contracts.GetCustomer;
using eShop.Customer.Contracts.UpdateCustomerGeneralInfo;

namespace eShop.ServiceInvocation.CustomerService.Refit;

public class CustomerService(ICustomerApi customerApi) : ICustomerService
{
    public async Task CreateCustomer(CreateCustomerDto dto)
    {
        await customerApi.CreateCustomer(dto);
    }

    public async Task DeleteCustomer(Guid objectId)
    {
        await customerApi.DeleteCustomer(objectId);
    }

    public async Task<CustomerDto> GetCustomer(Guid objectId)
    {
        return await customerApi.GetCustomer(objectId);
    }

    public async Task<CustomerDto> GetCustomer(string name)
    {
        return await customerApi.GetCustomer(name);
    }

    public async Task<Customer.Contracts.GetCustomers.CustomerDto[]> GetCustomers(bool includeDeleted = false)
    {
        return await customerApi.GetCustomers(includeDeleted);
    }

    public async Task UpdateCustomer(Guid objectId, UpdateCustomerDto dto)
    {
        await customerApi.UpdateCustomer(objectId, dto);
    }
}
