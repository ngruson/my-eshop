using Ardalis.Result;
using MediatR;

namespace eShop.AdminApp.Application.Queries.MasterData.GetCountries;

public record GetCountriesQuery : IRequest<Result<CountryViewModel[]>>;
