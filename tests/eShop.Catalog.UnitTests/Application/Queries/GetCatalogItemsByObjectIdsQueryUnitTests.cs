using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Queries.GetCatalogItemsByObjectIds;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Queries;

public class GetCatalogItemsByObjectIdsQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenCatalogItemsExist(
        GetCatalogItemsByObjectIdsQuery query,
        List<CatalogItem> catalogItems,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsByObjectIdsQueryHandler sut)
    {
        // Arrange

        repository.ListAsync(Arg.Any<GetCatalogItemsByIdsSpecification>(), default)
            .Returns(catalogItems);

        // Act

        Result<CatalogItemDto[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsByIdsSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundGivenNoCatalogItemsExist(
        GetCatalogItemsByObjectIdsQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsByObjectIdsQueryHandler sut)
    {
        // Arrange

        // Act

        Result<CatalogItemDto[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsByIdsSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogItemsByObjectIdsQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsByObjectIdsQueryHandler sut)
    {
        // Arrange

        repository.ListAsync(Arg.Any<GetCatalogItemsByIdsSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<CatalogItemDto[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsByIdsSpecification>(), default);
    }
}
