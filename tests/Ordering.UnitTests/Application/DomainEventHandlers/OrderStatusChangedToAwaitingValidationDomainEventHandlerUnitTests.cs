using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.DomainEventHandlers;
using eShop.Ordering.API.Application.IntegrationEvents.Events;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;

namespace Ordering.UnitTests.Application.DomainEventHandlers;
public class OrderStatusChangedToAwaitingValidationDomainEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task AddOrderStatusChangedToAwaitingValidationIntegrationEvent(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        [Substitute, Frozen] IRepository<Buyer> buyerRepository,
        [Substitute, Frozen] IIntegrationEventService integrationEventService,
        OrderStatusChangedToAwaitingValidationDomainEventHandler sut,
        OrderStatusChangedToAwaitingValidationDomainEvent evt,
        Order order,
        Buyer buyer)
    {
        // Arrange

        orderRepository.GetByIdAsync(evt.OrderId, default)
            .Returns(order);

        buyerRepository.GetByIdAsync(order.BuyerId.Value, default)
            .Returns(buyer);

        //Act

        await sut.Handle(evt, default);

        //Assert

        await integrationEventService.Received().AddAndSaveEventAsync(Arg.Any<OrderStatusChangedToAwaitingValidationIntegrationEvent>(), default);
    }
}
