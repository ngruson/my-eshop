using Ardalis.Result;
using eShop.MasterData.Contracts;
using MediatR;

namespace eShop.MasterData.API.Application.Queries.GetCountries;

public class GetCountriesQuery : IRequest<Result<List<CountryDto>>>
{
}
