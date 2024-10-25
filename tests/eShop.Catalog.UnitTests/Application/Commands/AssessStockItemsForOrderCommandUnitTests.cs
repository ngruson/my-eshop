using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Commands.AssessStockItemsForOrder;
using eShop.Catalog.API.IntegrationEvents.Events;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.EventBus.Events;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;
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
        AssessStockItemsForOrderCommandHandler sut,
        CatalogItem catalogItem)
    {
        // Arrange

        catalogItem.AvailableStock = command.OrderStockItems.Max(_ => _.Units);

        catalogItemRepository.SingleOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .Returns(catalogItem);

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

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
        AssessStockItemsForOrderCommandHandler sut,
        CatalogItem catalogItem)
    {
        // Arrange

        catalogItem.AvailableStock = command.OrderStockItems.Max(_ => _.Units) - 1;

        catalogItemRepository.SingleOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .Returns(catalogItem);

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

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

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());
        await catalogItemRepository.Received().SingleOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default);
        await integrationEventService.DidNotReceive().AddAndSaveEventAsync(Arg.Any<IntegrationEvent>(), default);
    }
}
