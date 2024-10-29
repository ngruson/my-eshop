using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.Catalog.GetCatalogItems;
using eShop.Catalog.Contracts;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.ServiceInvocation.CatalogService;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetCatalogItemsQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCatalogItemsExist(
        GetCatalogItemsQuery query,
        [Substitute, Frozen] ICatalogService catalogService,
        GetCatalogItemsQueryHandler sut,
        CatalogItemDto[] catalogItems)
    {
        // Arrange

        catalogService.GetCatalogItems()
            .Returns(catalogItems);

        // Act

        Result<AdminApp.Application.Queries.Catalog.GetCatalogItems.CatalogItemViewModel[]> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await catalogService.Received().GetCatalogItems();
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogItemsQuery query,
        [Substitute, Frozen] ICatalogService catalogService,
        GetCatalogItemsQueryHandler sut)
    {
        // Arrange

        catalogService.GetCatalogItems()
            .ThrowsAsync<Exception>();

        // Act

        Result<AdminApp.Application.Queries.Catalog.GetCatalogItems.CatalogItemViewModel[]> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await catalogService.Received().GetCatalogItems();
    }
}
