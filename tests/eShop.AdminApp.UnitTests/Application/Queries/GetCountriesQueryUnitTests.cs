using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.MasterData.GetCountries;
using eShop.MasterData.Contracts;
using eShop.ServiceInvocation.MasterDataApiClient;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetCountriesQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnCountriesGivenCacheMiss(
        GetCountriesQuery query,
        [Substitute, Frozen] IMasterDataApiClient masterDataApiClient,
        GetCountriesQueryHandler sut,
        CountryDto[] countries)
    {
        // Arrange

        masterDataApiClient.GetCountries()
            .Returns(countries);

        // Act

        Result<CountryViewModel[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await masterDataApiClient.Received().GetCountries();
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnCountriesGivenCacheHit(
        GetCountriesQuery query,
        [Substitute, Frozen] IMemoryCache cache,
        [Substitute, Frozen] IMasterDataApiClient masterDataApiClient,
        GetCountriesQueryHandler sut,
        CountryViewModel[] countries)
    {
        // Arrange

        cache.TryGetValue(Arg.Any<string>(), out object? cacheEntry).Returns(callInfo =>
        {
            callInfo[1] = countries;
            return true;
        });

        // Act

        Result<CountryViewModel[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await masterDataApiClient.DidNotReceive().GetCountries();
    }
}
