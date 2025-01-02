using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Commands.AssessStockItemsForOrder;
using eShop.Catalog.API.IntegrationEvents.Events;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Catalog.Contracts.AssessStockItemsForOrder;
using eShop.EventBus.Events;
using eShop.Shared.Data;
using eShop.Shared.Features;
using eShop.Shared.IntegrationEvents;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.Catalog.UnitTests.Application.Commands;

public class AssessStockItemsForOrderCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenStockIsConfirmed(
        AssessStockItemsForOrderCommand command,
        [Substitute, Frozen] IRepository<CatalogItem> catalogItemRepository,
        [Substitute, Frozen] IIntegrationEventService integrationEventService,
        [Substitute, Frozen] IOptions<FeaturesConfiguration> featuresOptions,
        [Substitute, Frozen] FeaturesConfiguration featuresConfiguration,
        AssessStockItemsForOrderCommandHandler sut,
        CatalogItem catalogItem)
    {
        // Arrange

        catalogItem.AvailableStock = command.Dto.OrderStockItems.Max(_ => _.Units);

        catalogItemRepository.SingleOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .Returns(catalogItem);

        featuresOptions.Value.Returns(featuresConfiguration);

        // Act

        Result<AssessStockItemsForOrderResponseDto> result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        await catalogItemRepository.Received().SingleOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default);
        await integrationEventService.Received().AddAndSaveEventAsync(Arg.Any<OrderStockConfirmedIntegrationEvent>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenStockIsRejected(
        AssessStockItemsForOrderCommand command,
        [Substitute, Frozen] IRepository<CatalogItem> catalogItemRepository,
        [Substitute, Frozen] IIntegrationEventService integrationEventService,
        [Substitute, Frozen] IOptions<FeaturesConfiguration> featuresOptions,
        [Substitute, Frozen] FeaturesConfiguration featuresConfiguration,
        AssessStockItemsForOrderCommandHandler sut,
        CatalogItem catalogItem)
    {
        // Arrange

        catalogItem.AvailableStock = command.Dto.OrderStockItems.Max(_ => _.Units) - 1;

        catalogItemRepository.SingleOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .Returns(catalogItem);

        featuresOptions.Value.Returns(featuresConfiguration);

        // Act

        Result<AssessStockItemsForOrderResponseDto> result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);
        await catalogItemRepository.Received().SingleOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default);
        await integrationEventService.Received().AddAndSaveEventAsync(Arg.Any<OrderStockRejectedIntegrationEvent>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        AssessStockItemsForOrderCommand command,
        [Substitute, Frozen] IRepository<CatalogItem> catalogItemRepository,
        [Substitute, Frozen] IIntegrationEventService integrationEventService,
        AssessStockItemsForOrderCommandHandler sut)
    {
        // Arrange

        catalogItemRepository.SingleOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .ThrowsAsync<Exception>();

        // Act

        Result<AssessStockItemsForOrderResponseDto> result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
        await catalogItemRepository.Received().SingleOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default);
        await integrationEventService.DidNotReceive().AddAndSaveEventAsync(Arg.Any<IntegrationEvent>(), default);
    }
}
