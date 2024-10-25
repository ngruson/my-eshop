using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Commands.CreateCatalogItem;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.CreateCatalogItem;
using eShop.Shared.Data;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Commands;

public class CreateCatalogItemCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCatalogItemIsCreated(
        CreateCatalogItemCommand command,
        CatalogType catalogType,
        CatalogBrand catalogBrand,
        [Substitute, Frozen] IRepository<CatalogItem> catalogItemRepository,
        [Substitute, Frozen] IRepository<CatalogType> catalogTypeRepository,
        [Substitute, Frozen] IRepository<CatalogBrand> catalogBrandRepository,
        CreateCatalogItemCommandHandler sut)
    {
        // Arrange

        catalogTypeRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default)
            .Returns(catalogType);
        catalogBrandRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogBrandByObjectIdSpecification>(), default)
            .Returns(catalogBrand);

        // Act

        Result<CatalogItemDto> result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        await catalogTypeRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default);
        await catalogBrandRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogBrandByObjectIdSpecification>(), default);
        await catalogItemRepository.Received().AddAsync(Arg.Any<CatalogItem>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundWhenCatalogTypeDoesNotExist(
        CreateCatalogItemCommand command,
        [Substitute, Frozen] IRepository<CatalogItem> catalogItemRepository,
        [Substitute, Frozen] IRepository<CatalogType> catalogTypeRepository,
        [Substitute, Frozen] IRepository<CatalogBrand> catalogBrandRepository,
        CreateCatalogItemCommandHandler sut)
    {
        // Arrange

        // Act

        Result<CatalogItemDto> result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
        await catalogTypeRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default);
        await catalogBrandRepository.DidNotReceive().FirstOrDefaultAsync(Arg.Any<GetCatalogBrandByObjectIdSpecification>(), default);
        await catalogItemRepository.DidNotReceive().AddAsync(Arg.Any<CatalogItem>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundWhenCatalogBrandDoesNotExist(
        CreateCatalogItemCommand command,
        CatalogType catalogType,
        [Substitute, Frozen] IRepository<CatalogItem> catalogItemRepository,
        [Substitute, Frozen] IRepository<CatalogType> catalogTypeRepository,
        [Substitute, Frozen] IRepository<CatalogBrand> catalogBrandRepository,
        CreateCatalogItemCommandHandler sut)
    {
        // Arrange

        catalogTypeRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default)
            .Returns(catalogType);

        // Act

        Result<CatalogItemDto> result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
        await catalogTypeRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default);
        await catalogBrandRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogBrandByObjectIdSpecification>(), default);
        await catalogItemRepository.DidNotReceive().AddAsync(Arg.Any<CatalogItem>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        CreateCatalogItemCommand command,
        CatalogType catalogType,
        CatalogBrand catalogBrand,
        [Substitute, Frozen] IRepository<CatalogItem> catalogItemRepository,
        [Substitute, Frozen] IRepository<CatalogType> catalogTypeRepository,
        [Substitute, Frozen] IRepository<CatalogBrand> catalogBrandRepository,
        CreateCatalogItemCommandHandler sut)
    {
        // Arrange

        catalogTypeRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default)
            .Returns(catalogType);
        catalogBrandRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogBrandByObjectIdSpecification>(), default)
            .Returns(catalogBrand);
        catalogItemRepository.AddAsync(Arg.Any<CatalogItem>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<CatalogItemDto> result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
        await catalogTypeRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default);
        await catalogBrandRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogBrandByObjectIdSpecification>(), default);
        await catalogItemRepository.Received().AddAsync(Arg.Any<CatalogItem>(), default);
    }
}
