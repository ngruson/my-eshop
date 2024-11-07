using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Queries.GetCardTypes;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Shared.Data;
using NSubstitute.ExceptionExtensions;

namespace eShop.Ordering.UnitTests.Application.Queries;

public class GetCardTypesQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task return_success_when_card_types_exists(
        [Substitute, Frozen] IRepository<CardType> repository,
        GetCardTypesQueryHandler sut,
        GetCardTypesQuery query,
        List<CardType> cardTypes
    )
    {
        // Arrange

        repository.ListAsync(default)
            .Returns(cardTypes);

        // Act

        Result<CardTypeDto[]> result = await sut.Handle(query, default);

        //Assert

        Assert.True(result.IsSuccess);
        Assert.Equal(cardTypes.Count, result.Value.Length);
    }

    [Theory, AutoNSubstituteData]
    internal async Task return_error_when_exception_is_thrown(
        [Substitute, Frozen] IRepository<CardType> repository,
        GetCardTypesQueryHandler sut,
        GetCardTypesQuery query
    )
    {
        // Arrange

        repository.ListAsync(default)
            .ThrowsAsync<Exception>();

        // Act

        Result<CardTypeDto[]> result = await sut.Handle(query, default);

        //Assert

        Assert.True(result.IsError());
    }
}
