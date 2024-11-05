using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.DomainEventHandlers;
using eShop.Ordering.API.Application.IntegrationEvents.Events;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;

namespace eShop.Ordering.UnitTests.Application.DomainEventHandlers;
public class OrderStatusChangedToAwaitingValidationDomainEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task AddOrderStatusChangedToAwaitingValidationIntegrationEvent(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        [Substitute, Frozen] IRepository<Buyer> buyerRepository,
        [Substitute, Frozen] IIntegrationEventService integrationEventService,
        OrderStatusChangedToAwaitingValidationDomainEventHandler sut,
        Order order,
        Buyer buyer)
    {
        // Arrange

        OrderItem[] orderItems =
        [
            new OrderItem(Guid.NewGuid(), "Product 1", 25, 0, null),
            new OrderItem(Guid.NewGuid(), "Product 2", 30, 0, null),
        ];


        OrderStatusChangedToAwaitingValidationDomainEvent evt = new(
            order.ObjectId,
            orderItems);

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        buyerRepository.GetByIdAsync(order.BuyerId.Value, default)
            .Returns(buyer);

        //Act

        await sut.Handle(evt, default);

        //Assert

        await integrationEventService.Received().AddAndSaveEventAsync(Arg.Any<OrderStatusChangedToAwaitingValidationIntegrationEvent>(), default);
    }
}
