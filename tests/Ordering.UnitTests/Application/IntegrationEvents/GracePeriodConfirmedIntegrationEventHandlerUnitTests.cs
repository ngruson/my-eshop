using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.IntegrationEvents.EventHandling;
using eShop.Ordering.API.Application.IntegrationEvents.Events;

namespace Ordering.UnitTests.Application.IntegrationEvents;
public class GracePeriodConfirmedIntegrationEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task PublishSetAwaitingValidationOrderStatusCommand(
            [Substitute, Frozen] IMediator mediator,
            GracePeriodConfirmedIntegrationEventHandler sut,
            GracePeriodConfirmedIntegrationEvent evt)
    {
        // Arrange

        //Act

        await sut.Handle(evt, default);

        //Assert

        await mediator.Send(Arg.Any<SetAwaitingValidationOrderStatusCommand>(), default);
    }
}
