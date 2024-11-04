using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Commands.SetStockConfirmedOrderStatus;
using eShop.Ordering.API.Application.IntegrationEvents.EventHandling;
using eShop.Ordering.API.Application.IntegrationEvents.Events;

namespace Ordering.UnitTests.Application.IntegrationEvents;
public class OrderStockConfirmedIntegrationEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task PublishSetStockConfirmedOrderStatusCommand(
        [Substitute, Frozen] IMediator mediator,
        OrderStockConfirmedIntegrationEventHandler sut,
        OrderStockConfirmedIntegrationEvent evt)
    {
        // Arrange

        //Act

        await sut.Handle(evt, default);

        //Assert

        await mediator.Send(Arg.Any<SetStockConfirmedOrderStatusCommand>(), default);
    }
}
