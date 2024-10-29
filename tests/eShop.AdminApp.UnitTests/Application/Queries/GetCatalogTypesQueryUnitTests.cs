using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.Catalog.GetCatalogTypes;
using eShop.Catalog.Contracts.GetCatalogTypes;
using eShop.ServiceInvocation.CatalogService;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetCatalogTypesQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCatalogTypesExist(
        GetCatalogTypesQuery query,
        [Substitute, Frozen] ICatalogService catalogService,
        GetCatalogTypesQueryHandler sut,
        CatalogTypeDto[] catalogTypes)
    {
        // Arrange

        catalogService.GetTypes()
            .Returns(catalogTypes);

        // Act

        Result<AdminApp.Application.Queries.Catalog.GetCatalogTypes.CatalogTypeViewModel[]> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await catalogService.Received().GetTypes();
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogTypesQuery query,
        [Substitute, Frozen] ICatalogService catalogService,
        GetCatalogTypesQueryHandler sut)
    {
        // Arrange

        catalogService.GetTypes()
            .ThrowsAsync<Exception>();

        // Act

        Result<AdminApp.Application.Queries.Catalog.GetCatalogTypes.CatalogTypeViewModel[]> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await catalogService.Received().GetTypes();
    }
}
