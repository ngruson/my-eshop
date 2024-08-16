using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.IntegrationEvents;
using eShop.Catalog.API.IntegrationEvents.EventHandling;
using eShop.Catalog.API.IntegrationEvents.Events;
using eShop.Catalog.API.Model;
using eShop.Shared.Data;
using NSubstitute;

namespace eShop.Catalog.UnitTests.IntegrationEvents;

public class OrderStatusChangedToAwaitingValidationIntegrationEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task when_stock_available_send_confirmed_event(
        [Substitute, Frozen] ICatalogIntegrationEventService catalogIntegrationEventService,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        OrderStatusChangedToAwaitingValidationIntegrationEventHandler sut,
        OrderStatusChangedToAwaitingValidationIntegrationEvent integrationEvent,
        CatalogItem catalogItem
    )
    {
        // Arrange

        catalogItem.AvailableStock = integrationEvent.OrderStockItems.Max(_ => _.Units);

        repository.GetByIdAsync(Arg.Any<int>(), default)
            .Returns(catalogItem);

        // Act

        await sut.Handle(integrationEvent, default);

        // Assert

        await catalogIntegrationEventService.Received().SaveEventAndDbChangesAsync(repository, Arg.Any<OrderStockConfirmedIntegrationEvent>());
        await catalogIntegrationEventService.Received().PublishThroughEventBusAsync(Arg.Any<OrderStockConfirmedIntegrationEvent>(), default);
    }

    [Theory, AutoNSubstituteData]
    internal async Task when_stock_not_available_send_rejected_event(
        [Substitute, Frozen] ICatalogIntegrationEventService catalogIntegrationEventService,
        [Substitute, Frozen] IRepository<CatalogItem> repository,
        OrderStatusChangedToAwaitingValidationIntegrationEventHandler sut,
        OrderStatusChangedToAwaitingValidationIntegrationEvent integrationEvent,
        CatalogItem catalogItem
    )
    {
        // Arrange

        catalogItem.AvailableStock = integrationEvent.OrderStockItems.Max(_ => _.Units) - 1;

        repository.GetByIdAsync(Arg.Any<int>(), default)
            .Returns(catalogItem);

        // Act

        await sut.Handle(integrationEvent, default);

        // Assert

        await catalogIntegrationEventService.Received().SaveEventAndDbChangesAsync(repository, Arg.Any<OrderStockRejectedIntegrationEvent>());
        await catalogIntegrationEventService.Received().PublishThroughEventBusAsync(Arg.Any<OrderStockRejectedIntegrationEvent>(), default);
    }
}
