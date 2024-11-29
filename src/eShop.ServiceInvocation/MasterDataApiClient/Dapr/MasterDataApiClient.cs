using Dapr.Client;
using eShop.MasterData.Contracts;
using eShop.ServiceInvocation.Auth;

namespace eShop.ServiceInvocation.MasterDataApiClient.Dapr;

public class MasterDataApiClient(DaprClient daprClient, AccessTokenAccessorFactory accessTokenAccessorFactory)
    : BaseDaprApiClient(daprClient, accessTokenAccessorFactory), IMasterDataApiClient
{
    protected override string AppId => "masterdata-api";

    public async Task<CountryDto[]> GetCountries()
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            "api/countries");

        CountryDto[] response = await this.DaprClient.InvokeMethodAsync<CountryDto[]>(request);

        return response;
    }

    public async Task<StateDto[]> GetStates()
    {
        HttpRequestMessage request = await this.CreateRequest(
            HttpMethod.Get,
            "api/states");

        StateDto[] response = await this.DaprClient.InvokeMethodAsync<StateDto[]>(request);

        return response;
    }
}
