using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.MasterData.GetStates;
using eShop.MasterData.Contracts;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetStatesQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnStatesGivenCacheMiss(
        GetStatesQuery query,
        [Substitute, Frozen] IMasterDataApi masterDataApi,
        GetStatesQueryHandler sut,
        StateDto[] states)
    {
        // Arrange

        masterDataApi.GetStates()
            .Returns(states);

        // Act

        Result<StateViewModel[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await masterDataApi.Received().GetStates();
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnStatesGivenCacheHit(
        GetStatesQuery query,
        [Substitute, Frozen] IMemoryCache cache,
        [Substitute, Frozen] IMasterDataApi masterDataApi,
        GetStatesQueryHandler sut,
        StateViewModel[] states)
    {
        // Arrange

        cache.TryGetValue(Arg.Any<string>(), out object? cacheEntry).Returns(callInfo =>
        {
            callInfo[1] = states;
            return true;
        });

        // Act

        Result<StateViewModel[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await masterDataApi.DidNotReceive().GetStates();
    }
}
