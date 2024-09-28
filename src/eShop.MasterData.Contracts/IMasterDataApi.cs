using Refit;

namespace eShop.MasterData.Contracts;

public interface IMasterDataApi
{
    [Get("/api/countries?api-version=1.0")]
    Task<CountryDto[]> GetCountries();

    [Get("/api/states?api-version=1.0")]
    Task<StateDto[]> GetStates();
}
