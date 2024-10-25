using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Queries.GetCatalogItemsByBrand;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Queries;

public class GetCatalogItemsByBrandQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenCatalogItemsExist(
        GetCatalogItemsByBrandQuery query,
        List<CatalogItem> catalogItems,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsByBrandQueryHandler sut)
    {
        // Arrange

        repository.ListAsync(Arg.Any<GetCatalogItemsByBrandSpecification>(), default)
            .Returns(catalogItems);
        repository.ListAsync(Arg.Any<GetCatalogItemsForPageByBrandSpecification>(), default)
            .Returns(catalogItems);

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        await repository.Received().CountAsync(Arg.Any<GetCatalogItemsByBrandSpecification>(), default);
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsForPageByBrandSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundGivenNoCatalogItemsExist(
        GetCatalogItemsByBrandQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsByBrandQueryHandler sut)
    {
        // Arrange

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
        await repository.Received().CountAsync(Arg.Any<GetCatalogItemsByBrandSpecification>(), default);
        await repository.Received().ListAsync(Arg.Any<GetCatalogItemsForPageByBrandSpecification>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogItemsByBrandQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemsByBrandQueryHandler sut)
    {
        // Arrange

        repository.CountAsync(Arg.Any<GetCatalogItemsByBrandSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<PaginatedItems<CatalogItemDto>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
    }
}
