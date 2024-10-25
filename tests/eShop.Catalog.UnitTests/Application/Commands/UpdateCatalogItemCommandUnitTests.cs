using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Commands.UpdateCatalogItem;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.EventBus.Events;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Commands;

public class UpdateCatalogItemCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenNoPriceChangeWhenCatalogItemIsUpdated(
        UpdateCatalogItemCommand command,
        CatalogItem catalogItem,
        CatalogType catalogType,
        CatalogBrand catalogBrand,
        [Substitute, Frozen] IRepository<CatalogItem> catalogItemRepository,
        [Substitute, Frozen] IRepository<CatalogType> catalogTypeRepository,
        [Substitute, Frozen] IRepository<CatalogBrand> catalogBrandRepository,
        [Substitute, Frozen] IIntegrationEventService eventService,
        UpdateCatalogItemCommandHandler sut)
    {
        // Arrange

        catalogItemRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .Returns(catalogItem);
        catalogTypeRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default)
            .Returns(catalogType);
        catalogBrandRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogBrandByObjectIdSpecification>(), default)
            .Returns(catalogBrand);

        // Act

        Result result = await sut.Handle(
            command with { Dto = command.Dto with { Price = catalogItem.Price } },
            default);

        // Assert

        Assert.True(result.IsSuccess);        
        await catalogItemRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default);
        await catalogTypeRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default);
        await catalogBrandRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogBrandByObjectIdSpecification>(), default);
        await catalogItemRepository.Received().UpdateAsync(catalogItem, default);
        await eventService.DidNotReceive().AddAndSaveEventAsync(Arg.Any<IntegrationEvent>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessGivenPriceChangeWhenCatalogItemIsUpdated(
        UpdateCatalogItemCommand command,
        CatalogItem catalogItem,
        CatalogType catalogType,
        CatalogBrand catalogBrand,
        [Substitute, Frozen] IRepository<CatalogItem> catalogItemRepository,
        [Substitute, Frozen] IRepository<CatalogType> catalogTypeRepository,
        [Substitute, Frozen] IRepository<CatalogBrand> catalogBrandRepository,
        [Substitute, Frozen] IIntegrationEventService eventService,
        UpdateCatalogItemCommandHandler sut)
    {
        // Arrange

        catalogItemRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .Returns(catalogItem);
        catalogTypeRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default)
            .Returns(catalogType);
        catalogBrandRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogBrandByObjectIdSpecification>(), default)
            .Returns(catalogBrand);

        // Act

        Result result = await sut.Handle(
            command with { Dto = command.Dto with { Price = catalogItem.Price + 1 } },
            default);

        // Assert

        Assert.True(result.IsSuccess);
        await catalogItemRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default);
        await catalogTypeRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default);
        await catalogBrandRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogBrandByObjectIdSpecification>(), default);
        await eventService.Received().AddAndSaveEventAsync(Arg.Any<IntegrationEvent>(), default);
        await catalogItemRepository.Received().UpdateAsync(catalogItem, default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundWhenCatalogItemDoesNotExist(
        UpdateCatalogItemCommand command,
        CatalogItem catalogItem,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        [Substitute, Frozen] IRepository<CatalogType> catalogTypeRepository,
        [Substitute, Frozen] IRepository<CatalogBrand> catalogBrandRepository,
        UpdateCatalogItemCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());        
        await repository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default);
        await catalogTypeRepository.DidNotReceive().FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default);
        await catalogBrandRepository.DidNotReceive().FirstOrDefaultAsync(Arg.Any<GetCatalogBrandByObjectIdSpecification>(), default);
        await repository.DidNotReceive().UpdateAsync(catalogItem, default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundWhenCatalogTypeDoesNotExist(
        UpdateCatalogItemCommand command,
        CatalogItem catalogItem,
        [Substitute, Frozen] IRepository<CatalogItem> catalogItemRepository,
        [Substitute, Frozen] IRepository<CatalogType> catalogTypeRepository,
        [Substitute, Frozen] IRepository<CatalogBrand> catalogBrandRepository,
        UpdateCatalogItemCommandHandler sut)
    {
        // Arrange

        catalogItemRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .Returns(catalogItem);

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
        await catalogItemRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default);
        await catalogTypeRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default);
        await catalogBrandRepository.DidNotReceive().FirstOrDefaultAsync(Arg.Any<GetCatalogBrandByObjectIdSpecification>(), default);
        await catalogItemRepository.DidNotReceive().UpdateAsync(catalogItem, default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnNotFoundWhenCatalogBrandDoesNotExist(
        UpdateCatalogItemCommand command,
        CatalogItem catalogItem,
        CatalogType catalogType,
        [Substitute, Frozen] IRepository<CatalogItem> catalogItemRepository,
        [Substitute, Frozen] IRepository<CatalogType> catalogTypeRepository,
        [Substitute, Frozen] IRepository<CatalogBrand> catalogBrandRepository,
        UpdateCatalogItemCommandHandler sut)
    {
        // Arrange

        catalogItemRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .Returns(catalogItem);
        catalogTypeRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default)
            .Returns(catalogType);

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsNotFound());
        await catalogItemRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default);
        await catalogTypeRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogTypeByObjectIdSpecification>(), default);
        await catalogBrandRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogBrandByObjectIdSpecification>(), default);
        await catalogItemRepository.DidNotReceive().UpdateAsync(catalogItem, default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        UpdateCatalogItemCommand command,
        CatalogItem catalogItem,
        [Substitute, Frozen] IRepository<CatalogItem> catalogItemRepository,
        UpdateCatalogItemCommandHandler sut)
    {
        // Arrange

        catalogItemRepository.FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
        await catalogItemRepository.Received().FirstOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default);
        await catalogItemRepository.DidNotReceive().UpdateAsync(catalogItem, default);
    }
}
