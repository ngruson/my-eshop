using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.MasterData.GetStates;
using eShop.MasterData.Contracts;
using eShop.ServiceInvocation.MasterDataService;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetStatesQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnStatesGivenCacheMiss(
        GetStatesQuery query,
        [Substitute, Frozen] IMasterDataService masterDataService,
        GetStatesQueryHandler sut,
        StateDto[] states)
    {
        // Arrange

        masterDataService.GetStates()
            .Returns(states);

        // Act

        Result<StateViewModel[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await masterDataService.Received().GetStates();
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnStatesGivenCacheHit(
        GetStatesQuery query,
        [Substitute, Frozen] IMemoryCache cache,
        [Substitute, Frozen] IMasterDataService masterDataService,
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

        await masterDataService.DidNotReceive().GetStates();
    }
}
