using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Queries.GetAllCatalogTypes;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogTypes;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Queries;

public class GetAllCatalogTypesQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenTypesExist(
        GetAllCatalogTypesQuery query,
        List<CatalogType> catalogTypes,
        [Substitute, Frozen] IRepository<CatalogType> repository,
        GetAllCatalogTypesQueryHandler sut)
    {
        // Arrange

        repository.ListAsync(Arg.Any<GetAllCatalogTypesSpecification>(), default)
            .Returns(catalogTypes);

        // Act

        Result<CatalogTypeDto[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundGivenNoTypesExist(
        GetAllCatalogTypesQuery query,
        GetAllCatalogTypesQueryHandler sut)
    {
        // Arrange        

        // Act

        Result<CatalogTypeDto[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetAllCatalogTypesQuery query,
        [Substitute, Frozen] IRepository<CatalogType> repository,
        GetAllCatalogTypesQueryHandler sut)
    {
        // Arrange        

        repository.ListAsync(Arg.Any<GetAllCatalogTypesSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<CatalogTypeDto[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
    }
}
