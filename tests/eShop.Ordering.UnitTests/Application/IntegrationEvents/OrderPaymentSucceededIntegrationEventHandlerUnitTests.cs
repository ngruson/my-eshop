using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.Commands.SetPaidOrderStatus;
using eShop.Ordering.API.Application.IntegrationEvents.EventHandling;
using eShop.Ordering.API.Application.IntegrationEvents.Events;

namespace Ordering.UnitTests.Application.IntegrationEvents;
public class OrderPaymentSucceededIntegrationEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task PublishSetPaidOrderStatusCommand(
        [Substitute, Frozen] IMediator mediator,
        OrderPaymentSucceededIntegrationEventHandler sut,
        OrderPaymentSucceededIntegrationEvent evt)
    {
        // Arrange

        //Act

        await sut.Handle(evt, default);

        //Assert

        await mediator.Send(Arg.Any<SetPaidOrderStatusCommand>(), default);
    }
}
