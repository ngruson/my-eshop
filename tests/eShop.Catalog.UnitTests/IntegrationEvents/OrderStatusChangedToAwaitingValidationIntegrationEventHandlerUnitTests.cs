using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Application.Commands.AssessStockItemsForOrder;
using eShop.Catalog.API.IntegrationEvents.EventHandling;
using eShop.Catalog.API.IntegrationEvents.Events;
using MediatR;
using NSubstitute;

namespace eShop.Catalog.UnitTests.IntegrationEvents;

public class OrderStatusChangedToAwaitingValidationIntegrationEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task HandleAssessStockItemsForOrderCommand(
        [Substitute, Frozen] IMediator mediator,
        OrderStatusChangedToAwaitingValidationIntegrationEventHandler sut,
        OrderStatusChangedToAwaitingValidationIntegrationEvent integrationEvent
    )
    {
        // Arrange        

        // Act

        await sut.Handle(integrationEvent, default);

        // Assert

        await mediator.Received().Send(Arg.Any<AssessStockItemsForOrderCommand>(), default);
    }
}
