using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Queries.GetCatalogItemPictureByObjectId;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Queries;

public class GetCatalogItemPictureByObjectIdQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenCatalogItemExists(
        GetCatalogItemPictureByObjectIdQuery query,
        CatalogItem catalogItem,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemPictureByObjectIdQueryHandler sut)
    {
        // Arrange

        repository.FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .Returns(catalogItem);

        // Act

        Result<PictureDto> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        Assert.Contains(catalogItem.PictureFileName!, result.Value.Path);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundGivenCatalogItemDoesNotExist(
        GetCatalogItemPictureByObjectIdQuery query,
        GetCatalogItemPictureByObjectIdQueryHandler sut)
    {
        // Arrange

        // Act

        Result<PictureDto> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());        
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCatalogItemPictureByObjectIdQuery query,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        GetCatalogItemPictureByObjectIdQueryHandler sut)
    {
        // Arrange

        repository.FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<PictureDto> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
    }
}
