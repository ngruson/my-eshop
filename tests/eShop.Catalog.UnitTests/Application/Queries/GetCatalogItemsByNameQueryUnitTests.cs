using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Queries.GetCatalogItemsByName;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Queries;

public class GetCatalogItemsByNameQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenCatalogItemsExist(
        GetCatalogItemsByNameQuery query,
        List<CatalogItem> catalogItems,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsByNameQueryHandler sut)
    {
        // Arrange

        repository.ListAsync(Arg.Any<GetCatalogItemsForPageStartingWithNameSpecification>(), default)
            .Returns(catalogItems);

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        await repository.Received().CountAsync(Arg.Any<GetCatalogItemsStartingWithNameSpecification>(), default);
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsForPageStartingWithNameSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundGivenNoCatalogItemsExist(
        GetCatalogItemsByNameQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsByNameQueryHandler sut)
    {
        // Arrange

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
        await repository.Received().CountAsync(Arg.Any<GetCatalogItemsStartingWithNameSpecification>(), default);
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsForPageStartingWithNameSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogItemsByNameQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsByNameQueryHandler sut)
    {
        // Arrange

        repository.CountAsync(Arg.Any<GetCatalogItemsStartingWithNameSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
    }
}
