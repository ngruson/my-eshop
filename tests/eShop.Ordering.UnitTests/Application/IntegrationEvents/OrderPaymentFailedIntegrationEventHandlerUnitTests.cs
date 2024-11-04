using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Commands.CancelOrder;
using eShop.Ordering.API.Application.IntegrationEvents.EventHandling;
using eShop.Ordering.API.Application.IntegrationEvents.Events;

namespace Ordering.UnitTests.Application.IntegrationEvents;
public class OrderPaymentFailedIntegrationEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task PublishCancelOrderCommand(
            [Substitute, Frozen] IMediator mediator,
            OrderPaymentFailedIntegrationEventHandler sut,
            OrderPaymentFailedIntegrationEvent evt)
    {
        // Arrange

        //Act

        await sut.Handle(evt, default);

        //Assert

        await mediator.Send(Arg.Any<CancelOrderCommand>(), default);
    }
}
