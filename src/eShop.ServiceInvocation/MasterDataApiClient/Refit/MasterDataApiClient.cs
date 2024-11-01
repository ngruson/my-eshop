using eShop.MasterData.Contracts;

namespace eShop.ServiceInvocation.MasterDataApiClient.Refit;

public class MasterDataApiClient(IMasterDataApi masterDataApi) : IMasterDataApiClient
{
    public async Task<CountryDto[]> GetCountries()
    {
        return await masterDataApi.GetCountries();
    }

    public async Task<StateDto[]> GetStates()
    {
        return await masterDataApi.GetStates();
    }
}
