using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.Catalog.GetCatalogItem;
using eShop.Catalog.Contracts;
using eShop.Catalog.Contracts.GetCatalogItem;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetCatalogItemQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCatalogItemExist(
        GetCatalogItemQuery query,
        [Substitute, Frozen] ICatalogApi catalogApi,
        GetCatalogItemQueryHandler sut,
        CatalogItemDto catalogItem)
    {
        // Arrange

        catalogApi.GetCatalogItem(query.ObjectId)
            .Returns(catalogItem);

        // Act

        Result<CatalogItemViewModel> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await catalogApi.Received().GetCatalogItem(query.ObjectId);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogItemQuery query,
        [Substitute, Frozen] ICatalogApi catalogApi,
        GetCatalogItemQueryHandler sut)
    {
        // Arrange

        catalogApi.GetCatalogItem(query.ObjectId)
            .ThrowsAsync<Exception>();

        // Act

        Result<CatalogItemViewModel> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await catalogApi.Received().GetCatalogItem(query.ObjectId);
    }
}
