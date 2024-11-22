using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Basket.API.IntegrationEvents.EventHandling;
using eShop.Basket.API.IntegrationEvents.Events;
using eShop.Basket.API.Repositories;
using Xunit;

namespace Basket.UnitTests.IntegrationEvents;
public class OrderStartedIntegrationEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task DeleteBasket(
        [Substitute, Frozen] IBasketRepository basketRepository,
        OrderStartedIntegrationEventHandler sut,
        OrderStartedIntegrationEvent evt)
    {
        // Arrange

        //Act

        await sut.Handle(evt, default);

        //Assert

        await basketRepository.DeleteBasketAsync(evt.UserId);
    }
}
