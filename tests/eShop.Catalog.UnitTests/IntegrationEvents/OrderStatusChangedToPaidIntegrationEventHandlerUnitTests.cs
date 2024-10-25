using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.IntegrationEvents.EventHandling;
using eShop.Catalog.API.IntegrationEvents.Events;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Specifications;
using eShop.Shared.Data;
using NSubstitute;

namespace eShop.Catalog.UnitTests.IntegrationEvents;

public class OrderStatusChangedToPaidIntegrationEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task remove_stock(
    [Substitute, Frozen] IRepository<CatalogItem> repository,
        OrderStatusChangedToPaidIntegrationEventHandler sut,
        OrderStatusChangedToPaidIntegrationEvent integrationEvent,
        CatalogItem[] catalogItems
    )
    {
        // Arrange

        foreach (CatalogItem catalogItem in catalogItems)
        {
            catalogItem.AvailableStock = integrationEvent.OrderStockItems.Max(_ => _.Units);
        }

        repository.SingleOrDefaultAsync(Arg.Any<GetCatalogItemByObjectIdSpecification>(), default)
            .Returns(
                _ => catalogItems[0],
                _ => catalogItems[1],
                _ => catalogItems[2]);

        // Act

        await sut.Handle(integrationEvent, default);

        // Assert

        await repository.Received().UpdateAsync(catalogItems[0], default);
        await repository.Received().UpdateAsync(catalogItems[1], default);
        await repository.Received().UpdateAsync(catalogItems[2], default);
    }
}
