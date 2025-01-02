using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.DomainEventHandlers;
using eShop.Ordering.API.Application.IntegrationEvents.Events;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;
using eShop.Shared.Features;
using eShop.Shared.IntegrationEvents;
using Microsoft.Extensions.Options;

namespace Ordering.UnitTests.Application.DomainEventHandlers;
public class OrderStatusChangedToStockConfirmedDomainEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task AddOrderStatusChangedToStockConfirmedIntegrationEvent(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        [Substitute, Frozen] IRepository<Buyer> buyerRepository,
        [Substitute, Frozen] IIntegrationEventService integrationEventService,
        [Substitute, Frozen] IOptions<FeaturesConfiguration> featuresOptions,
        [Substitute, Frozen] FeaturesConfiguration featuresConfiguration,
        OrderStatusChangedToStockConfirmedDomainEventHandler sut,
        OrderStatusChangedToStockConfirmedDomainEvent evt,
        Order order,
        Buyer buyer)
    {
        // Arrange

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        buyerRepository.GetByIdAsync(order.BuyerId.Value, default)
            .Returns(buyer);

        featuresOptions.Value.Returns(featuresConfiguration);

        //Act

        await sut.Handle(evt, default);

        //Assert

        await integrationEventService.Received().AddAndSaveEventAsync(Arg.Any<OrderStatusChangedToStockConfirmedIntegrationEvent>(), default);
    }
}
