using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Queries.GetAllCatalogItems;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Queries;

public class GetAllCatalogItemsQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenCatalogTypesExist(
        GetAllCatalogItemsQuery query,
        List<CatalogItem> catalogItems,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetAllCatalogItemsQueryHandler sut)
    {
        // Arrange

        repository.ListAsync(Arg.Any<GetCatalogItemsSpecification>(), default)
            .Returns(catalogItems);

        // Act

        Result<CatalogItemDto[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundGivenNoCatalogTypesExist(
        GetAllCatalogItemsQuery query,
        GetAllCatalogItemsQueryHandler sut)
    {
        // Arrange

        // Act

        Result<CatalogItemDto[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetAllCatalogItemsQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetAllCatalogItemsQueryHandler sut)
    {
        // Arrange

        repository.ListAsync(Arg.Any<GetCatalogItemsSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<CatalogItemDto[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
    }
}
