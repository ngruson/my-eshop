using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Queries.GetCatalogItemsByTypeAndBrand;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Queries;

public class GetCatalogItemsByTypeAndBrandQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenCatalogItemsExist(
        GetCatalogItemsByTypeAndBrandQuery query,
        List<CatalogItem> catalogItems,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsByTypeAndBrandQueryHandler sut)
    {
        // Arrange

        repository.CountAsync(Arg.Any<GetCatalogItemsByTypeAndBrandSpecification>(), default)
            .Returns(catalogItems.Count);
        repository.ListAsync(Arg.Any<GetCatalogItemsForPageByTypeAndBrandSpecification>(), default)
            .Returns(catalogItems);

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        await repository.Received().CountAsync(Arg.Any<GetCatalogItemsByTypeAndBrandSpecification>(), default);
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsForPageByTypeAndBrandSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundGivenNoCatalogItemsExist(
        GetCatalogItemsByTypeAndBrandQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsByTypeAndBrandQueryHandler sut)
    {
        // Arrange

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
        await repository.Received().CountAsync(Arg.Any<GetCatalogItemsByTypeAndBrandSpecification>(), default);
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsForPageByTypeAndBrandSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogItemsByTypeAndBrandQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsByTypeAndBrandQueryHandler sut)
    {
        // Arrange

        repository.CountAsync(Arg.Any<GetCatalogItemsByTypeAndBrandSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
    }
}
