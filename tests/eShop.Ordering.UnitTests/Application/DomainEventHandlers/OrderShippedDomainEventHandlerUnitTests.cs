using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.DomainEventHandlers;
using eShop.Ordering.API.Application.IntegrationEvents.Events;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;

namespace Ordering.UnitTests.Application.DomainEventHandlers;
public class OrderShippedDomainEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task AddOrderStatusChangedToShippedIntegrationEvent(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        [Substitute, Frozen] IRepository<Buyer> buyerRepository,
        [Substitute, Frozen] IIntegrationEventService integrationEventService,
        OrderShippedDomainEventHandler sut,
        OrderShippedDomainEvent evt,
        Order order,
        Buyer buyer)
    {
        // Arrange

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        buyerRepository.GetByIdAsync(order.BuyerId.Value, default)
            .Returns(buyer);

        //Act

        await sut.Handle(evt, default);

        //Assert

        await integrationEventService.Received().AddAndSaveEventAsync(Arg.Any<OrderStatusChangedToShippedIntegrationEvent>(), default);
    }
}
