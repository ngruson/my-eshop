using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.Catalog.GetCatalogTypes;
using eShop.Catalog.Contracts.GetCatalogTypes;
using eShop.ServiceInvocation.CatalogApiClient;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetCatalogTypesQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCatalogTypesExist(
        GetCatalogTypesQuery query,
        [Substitute, Frozen] ICatalogApiClient catalogApiClient,
        GetCatalogTypesQueryHandler sut,
        CatalogTypeDto[] catalogTypes)
    {
        // Arrange

        catalogApiClient.GetTypes()
            .Returns(catalogTypes);

        // Act

        Result<AdminApp.Application.Queries.Catalog.GetCatalogTypes.CatalogTypeViewModel[]> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await catalogApiClient.Received().GetTypes();
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogTypesQuery query,
        [Substitute, Frozen] ICatalogApiClient catalogApiClient,
        GetCatalogTypesQueryHandler sut)
    {
        // Arrange

        catalogApiClient.GetTypes()
            .ThrowsAsync<Exception>();

        // Act

        Result<AdminApp.Application.Queries.Catalog.GetCatalogTypes.CatalogTypeViewModel[]> result =
            await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await catalogApiClient.Received().GetTypes();
    }
}
