using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.DomainEventHandlers;
using eShop.Ordering.API.Application.IntegrationEvents.Events;
using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;

namespace eShop.Ordering.UnitTests.Application.DomainEventHandlers;
public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task WhenBuyerExists_AddOrderStatusChangedToSubmittedIntegrationEvent(
        [Substitute, Frozen] IRepository<Buyer> buyerRepository,
        [Substitute, Frozen] IIntegrationEventService integrationEventService,
        ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler sut,
        OrderStartedDomainEvent evt,
        Buyer buyer)
    {
        // Arrange

        // Add AutoFixture customization later
        OrderStartedDomainEvent copy = new(evt.Order, evt.UserId, evt.UserName,
            evt.CardType, evt.CardNumber, evt.CardSecurityNumber, evt.CardHolderName,
            DateTime.Now.AddYears(1));

        buyerRepository.SingleOrDefaultAsync(Arg.Any<GetBuyerByIdentitySpecification>(), default)
            .Returns(buyer);

        //Act

        await sut.Handle(copy, default);

        //Assert

        await buyerRepository.Received().UpdateAsync(buyer, default);
        await integrationEventService.AddAndSaveEventAsync(Arg.Any<OrderStatusChangedToSubmittedIntegrationEvent>(), default);

        await buyerRepository.DidNotReceive().AddAsync(buyer, default);
    }

    [Theory, AutoNSubstituteData]
    public async Task WhenBuyerDoesNotExist_AddBuyerAndAddOrderStatusChangedToSubmittedIntegrationEvent(
        [Substitute, Frozen] IRepository<Buyer> buyerRepository,
        [Substitute, Frozen] IIntegrationEventService integrationEventService,
        ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler sut,
        OrderStartedDomainEvent evt)
    {
        // Arrange

        OrderStartedDomainEvent copyEvent = new OrderStartedDomainEvent(
            evt.Order,
            evt.UserId,
            evt.UserName,
            evt.CardType,
            evt.CardNumber,
            evt.CardSecurityNumber,
            evt.CardHolderName,
            DateTime.Now.AddYears(1));

        //Act

        await sut.Handle(copyEvent, default);

        //Assert

        await buyerRepository.Received().AddAsync(Arg.Any<Buyer>(), default);
        await integrationEventService.AddAndSaveEventAsync(Arg.Any<OrderStatusChangedToSubmittedIntegrationEvent>(), default);
    }
}
