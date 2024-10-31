using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.Catalog.GetCatalogItems;
using eShop.Catalog.Contracts;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.ServiceInvocation.CatalogApiClient;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetCatalogItemsQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCatalogItemsExist(
        GetCatalogItemsQuery query,
        [Substitute, Frozen] ICatalogApiClient catalogApiClient,
        GetCatalogItemsQueryHandler sut,
        CatalogItemDto[] catalogItems)
    {
        // Arrange

        catalogApiClient.GetCatalogItems()
            .Returns(catalogItems);

        // Act

        Result<AdminApp.Application.Queries.Catalog.GetCatalogItems.CatalogItemViewModel[]> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await catalogApiClient.Received().GetCatalogItems();
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogItemsQuery query,
        [Substitute, Frozen] ICatalogApiClient catalogApiClient,
        GetCatalogItemsQueryHandler sut)
    {
        // Arrange

        catalogApiClient.GetCatalogItems()
            .ThrowsAsync<Exception>();

        // Act

        Result<AdminApp.Application.Queries.Catalog.GetCatalogItems.CatalogItemViewModel[]> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await catalogApiClient.Received().GetCatalogItems();
    }
}
