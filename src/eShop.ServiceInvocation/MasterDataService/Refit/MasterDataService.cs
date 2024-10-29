using eShop.MasterData.Contracts;

namespace eShop.ServiceInvocation.MasterDataService.Refit;

public class MasterDataService(IMasterDataApi masterDataApi) : IMasterDataService
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
