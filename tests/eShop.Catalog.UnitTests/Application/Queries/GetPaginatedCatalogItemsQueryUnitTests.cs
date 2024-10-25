using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Queries.GetPaginatedCatalogItems;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Queries;

public class GetPaginatedCatalogItemsQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenCatalogItemsExist(
        GetPaginatedCatalogItemsQuery query,
        List<CatalogItem> catalogItems,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetPaginatedCatalogItemsQueryHandler sut)
    {
        // Arrange

        repository.CountAsync(Arg.Any<GetCatalogItemsSpecification>(), default)
            .Returns(catalogItems.Count);
        repository.ListAsync(Arg.Any<GetCatalogItemsForPageSpecification>(), default)
            .Returns(catalogItems);

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        Assert.Equal(catalogItems.Count, result.Value.Count);
        await repository.Received().CountAsync(Arg.Any<GetCatalogItemsSpecification>(), default);
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsForPageSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundGivenNoCatalogItemsExist(
        GetPaginatedCatalogItemsQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetPaginatedCatalogItemsQueryHandler sut)
    {
        // Arrange        

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
        await repository.Received().CountAsync(Arg.Any<GetCatalogItemsSpecification>(), default);
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsForPageSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetPaginatedCatalogItemsQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetPaginatedCatalogItemsQueryHandler sut)
    {
        // Arrange

        repository.CountAsync(Arg.Any<GetCatalogItemsSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
        await repository.Received().CountAsync(Arg.Any<GetCatalogItemsSpecification>(), default);
    }
}
