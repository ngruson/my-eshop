using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.IntegrationEvents.EventHandling;
using eShop.Ordering.API.Application.IntegrationEvents.Events;

namespace Ordering.UnitTests.Application.IntegrationEvents;
public class OrderStockRejectedIntegrationEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task PublishSetStockRejectedOrderStatusCommand(
        [Substitute, Frozen] IMediator mediator,
        OrderStockRejectedIntegrationEventHandler sut,
        OrderStockRejectedIntegrationEvent evt)
    {
        // Arrange

        //Act

        await sut.Handle(evt, default);

        //Assert

        await mediator.Send(Arg.Any<SetStockRejectedOrderStatusCommand>(), default);
    }
}
