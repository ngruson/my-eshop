using eShop.MasterData.Contracts;

namespace eShop.ServiceInvocation.MasterDataApiClient;

public interface IMasterDataApiClient
{
    Task<CountryDto[]> GetCountries();

    Task<StateDto[]> GetStates();
}
