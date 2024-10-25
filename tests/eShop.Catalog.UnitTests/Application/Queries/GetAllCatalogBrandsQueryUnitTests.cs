using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Queries.GetAllCatalogBrands;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogBrands;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Queries;

public class GetAllCatalogBrandsQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenBrandsExist(
        GetAllCatalogBrandsQuery query,
        List<CatalogBrand> catalogBrands,
        [Substitute, Frozen] IRepository<CatalogBrand> repository,
        GetAllCatalogBrandsQueryHandler sut)
    {
        // Arrange

        repository.ListAsync(Arg.Any<GetAllCatalogBrandsSpecification>(), default)
            .Returns(catalogBrands);

        // Act

        Result<CatalogBrandDto[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundGivenNoBrandsExist(
        GetAllCatalogBrandsQuery query,
        GetAllCatalogBrandsQueryHandler sut)
    {
        // Arrange        

        // Act

        Result<CatalogBrandDto[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetAllCatalogBrandsQuery query,
        [Substitute, Frozen] IRepository<CatalogBrand> repository,
        GetAllCatalogBrandsQueryHandler sut)
    {
        // Arrange        

        repository.ListAsync(Arg.Any<GetAllCatalogBrandsSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<CatalogBrandDto[]> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
    }
}
