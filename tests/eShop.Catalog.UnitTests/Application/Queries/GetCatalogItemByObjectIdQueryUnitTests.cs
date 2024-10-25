using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Queries.GetCatalogItemByObjectId;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.GetCatalogItem;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Queries;

public class GetCatalogItemByObjectIdQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenCatalogTypesExist(
        GetCatalogItemByObjectIdQuery query,
        CatalogItem catalogItem,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemByObjectIdQueryHandler sut)
    {
        // Arrange

        repository.FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .Returns(catalogItem);

        // Act

        Result<CatalogItemDto> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundGivenCatalogItemDoesNotExist(
        GetCatalogItemByObjectIdQuery query,
        GetCatalogItemByObjectIdQueryHandler sut)
    {
        // Arrange

        // Act

        Result<CatalogItemDto> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogItemByObjectIdQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemByObjectIdQueryHandler sut)
    {
        // Arrange

        repository.FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<CatalogItemDto> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
    }
}
