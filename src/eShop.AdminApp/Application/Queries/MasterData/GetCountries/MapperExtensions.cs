using eShop.MasterData.Contracts;

namespace eShop.AdminApp.Application.Queries.MasterData.GetCountries;

internal static class MapperExtensions
{
    public static CountryViewModel[] MapToCountryViewModels(this CountryDto[] countries)
    {
        return countries
            .Select(c => new CountryViewModel(c.Code, c.Name))
            .ToArray();
    }
}
