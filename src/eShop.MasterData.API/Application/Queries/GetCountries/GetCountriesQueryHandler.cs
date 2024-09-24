using Ardalis.Result;
using eShop.MasterData.Contracts;
using MediatR;

namespace eShop.MasterData.API.Application.Queries.GetCountries;

public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, Result<List<CountryDto>>>
{
    public Task<Result<List<CountryDto>>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        List<CountryDto> countries = CountryEnum.List
            .Select(c => new CountryDto(c.Name, c.Value))
            .ToList();

        return Task.FromResult(Result<List<CountryDto>>.Success(countries));
    }
}
