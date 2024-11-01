using Dapr.Client;
using eShop.MasterData.Contracts;
using eShop.Shared.Auth;

namespace eShop.ServiceInvocation.MasterDataApiClient.Dapr;

public class MasterDataApiClient(DaprClient daprClient, AccessTokenAccessor accessTokenAccessor)
    : BaseDaprApiClient(daprClient, accessTokenAccessor), IMasterDataApiClient
{
    protected override string AppId => "masterData-api";

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
