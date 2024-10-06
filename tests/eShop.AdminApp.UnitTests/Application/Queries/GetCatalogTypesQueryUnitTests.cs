using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.Catalog.GetCatalogTypes;
using eShop.Catalog.Contracts;
using eShop.Catalog.Contracts.GetCatalogTypes;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetCatalogTypesQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCatalogTypesExist(
        GetCatalogTypesQuery query,
        [Substitute, Frozen] ICatalogApi catalogApi,
        GetCatalogTypesQueryHandler sut,
        CatalogTypeDto[] catalogTypes)
    {
        // Arrange

        catalogApi.GetCatalogTypes()
            .Returns(catalogTypes);

        // Act

        Result<CatalogTypeViewModel[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await catalogApi.Received().GetCatalogTypes();
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogTypesQuery query,
        [Substitute, Frozen] ICatalogApi catalogApi,
        GetCatalogTypesQueryHandler sut)
    {
        // Arrange

        catalogApi.GetCatalogTypes()
            .ThrowsAsync<Exception>();

        // Act

        Result<CatalogTypeViewModel[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await catalogApi.Received().GetCatalogTypes();
    }
}
