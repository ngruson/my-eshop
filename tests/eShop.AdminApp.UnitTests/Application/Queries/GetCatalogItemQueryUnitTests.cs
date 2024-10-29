using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.Catalog.GetCatalogItem;
using eShop.Catalog.Contracts;
using eShop.ServiceInvocation.CatalogService;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetCatalogItemQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCatalogItemExist(
        GetCatalogItemQuery query,
        [Substitute, Frozen] ICatalogService catalogService,
        GetCatalogItemQueryHandler sut,
        ServiceInvocation.CatalogService.CatalogItemViewModel catalogItem)
    {
        // Arrange

        catalogService.GetCatalogItem(query.ObjectId)
            .Returns(catalogItem);

        // Act

        Result<AdminApp.Application.Queries.Catalog.GetCatalogItem.CatalogItemViewModel> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await catalogService.Received().GetCatalogItem(query.ObjectId);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogItemQuery query,
        [Substitute, Frozen] ICatalogService catalogService,
        GetCatalogItemQueryHandler sut)
    {
        // Arrange

        catalogService.GetCatalogItem(query.ObjectId)
            .ThrowsAsync<Exception>();

        // Act

        Result<AdminApp.Application.Queries.Catalog.GetCatalogItem.CatalogItemViewModel> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await catalogService.Received().GetCatalogItem(query.ObjectId);
    }
}
