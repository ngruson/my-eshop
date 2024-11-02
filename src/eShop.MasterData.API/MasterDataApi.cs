using Ardalis.Result.AspNetCore;
using eShop.MasterData.API.Application.Queries.GetCountries;
using eShop.MasterData.API.Application.Queries.GetStates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eShop.MasterData.API;

internal static class MasterDataApi
{
    public static RouteGroupBuilder MapCountryApiV1(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.MapGroup("api/countries").HasApiVersion(1.0);

        api.MapGet("/", async ([FromServices] IMediator mediator) =>
            (await mediator.Send(new GetCountriesQuery()))
                .ToMinimalApiResult());

        return api;
    }

    public static RouteGroupBuilder MapStateApiV1(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.MapGroup("api/states").HasApiVersion(1.0);

        api.MapGet("/", async ([FromServices] IMediator mediator) =>
            (await mediator.Send(new GetStatesQuery()))
                .ToMinimalApiResult());

        return api;
    }
}
