using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.Catalog.GetCatalogItems;
using eShop.Catalog.Contracts;
using eShop.Catalog.Contracts.GetCatalogItems;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetCatalogItemsQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCatalogItemsExist(
        GetCatalogItemsQuery query,
        [Substitute, Frozen] ICatalogApi catalogApi,
        GetCatalogItemsQueryHandler sut,
        CatalogItemDto[] catalogItems)
    {
        // Arrange

        catalogApi.GetCatalogItems()
            .Returns(catalogItems);

        // Act

        Result<CatalogItemViewModel[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await catalogApi.Received().GetCatalogItems();
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogItemsQuery query,
        [Substitute, Frozen] ICatalogApi catalogApi,
        GetCatalogItemsQueryHandler sut)
    {
        // Arrange

        catalogApi.GetCatalogItems()
            .ThrowsAsync<Exception>();

        // Act

        Result<CatalogItemViewModel[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await catalogApi.Received().GetCatalogItems();
    }
}
