using Dapr.Client;
using eShop.Customer.Contracts.CreateCustomer;
using eShop.Customer.Contracts.UpdateCustomerGeneralInfo;
using eShop.ServiceInvocation.Auth;

namespace eShop.ServiceInvocation.CustomerApiClient.Dapr;

public class CustomerApiClient(DaprClient daprClient, AccessTokenAccessorFactory accessTokenAccessorFactory)
    : BaseDaprApiClient(daprClient, accessTokenAccessorFactory), ICustomerApiClient
{
    private readonly string basePath = "api/customers/";
    protected override string AppId => "customer-api";

    public async Task CreateCustomer(CreateCustomerDto dto)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Post,
            this.basePath,
            null,
            dto);

        await this.DaprClient.InvokeMethodAsync(request);
    }

    public async Task DeleteCustomer(Guid objectId)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Delete,
            $"{this.basePath}{objectId}");

        await this.DaprClient.InvokeMethodAsync(request);
    }

    public async Task<Customer.Contracts.GetCustomer.CustomerDto> GetCustomer(Guid objectId)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            $"{this.basePath}{objectId}");

        Customer.Contracts.GetCustomer.CustomerDto response =
            await this.DaprClient.InvokeMethodAsync<Customer.Contracts.GetCustomer.CustomerDto>(
                request);

        return response;
    }

    public async Task<Customer.Contracts.GetCustomer.CustomerDto> GetCustomer(string name)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            $"{this.basePath}name/{name}");

        Customer.Contracts.GetCustomer.CustomerDto response =
            await this.DaprClient.InvokeMethodAsync<Customer.Contracts.GetCustomer.CustomerDto>(
                request);

        return response;
    }

    public async Task<Customer.Contracts.GetCustomers.CustomerDto[]> GetCustomers(bool includeDeleted = false)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            this.basePath,
            [new KeyValuePair<string, string>("includeDeleted", includeDeleted.ToString())]);

        Customer.Contracts.GetCustomers.CustomerDto[] response =
            await this.DaprClient.InvokeMethodAsync<Customer.Contracts.GetCustomers.CustomerDto[]>(
                request);

        return response;
    }

    public async Task UpdateCustomer(Guid objectId, UpdateCustomerDto dto)
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Put,
            $"{this.basePath}{objectId}/generalInfo",
            null,
            dto);

        await this.DaprClient.InvokeMethodAsync(request);
    }
}
