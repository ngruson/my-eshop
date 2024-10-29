using eShop.MasterData.Contracts;

namespace eShop.ServiceInvocation.MasterDataService;

public interface IMasterDataService
{
    Task<CountryDto[]> GetCountries();

    Task<StateDto[]> GetStates();
}
