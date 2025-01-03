using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.DomainEventHandlers;
using eShop.Ordering.API.Application.IntegrationEvents.Events;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Ordering.Domain.AggregatesModel.SalesTaxRateAggregate;
using eShop.Shared.Data;
using eShop.Shared.Features;
using eShop.Shared.IntegrationEvents;
using Microsoft.Extensions.Options;

namespace eShop.Ordering.UnitTests.Application.DomainEventHandlers;
public class OrderStatusChangedToAwaitingValidationDomainEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task AddOrderStatusChangedToAwaitingValidationIntegrationEvent(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        [Substitute, Frozen] IRepository<Buyer> buyerRepository,
        [Substitute, Frozen] IIntegrationEventService integrationEventService,
        [Substitute, Frozen] IOptions<FeaturesConfiguration> featuresOptions,
        [Substitute, Frozen] FeaturesConfiguration featuresConfiguration,
        OrderStatusChangedToAwaitingValidationDomainEventHandler sut,
        Order order,
        Buyer buyer,
        SalesTaxRate salesTaxRate)
    {
        // Arrange

        OrderItem[] orderItems =
        [
            new OrderItem(Guid.NewGuid(), "Product 1", 25, salesTaxRate.Rate, 0, null),
            new OrderItem(Guid.NewGuid(), "Product 2", 30, salesTaxRate.Rate, 0, null),
        ];

        OrderStatusChangedToAwaitingValidationDomainEvent evt = new(
            order.ObjectId,
            orderItems);

        orderRepository.SingleOrDefaultAsync(Arg.Any<GetOrderSpecification>(), default)
            .Returns(order);

        buyerRepository.GetByIdAsync(order.BuyerId.Value, default)
            .Returns(buyer);

        featuresOptions.Value.Returns(featuresConfiguration);

        //Act

        await sut.Handle(evt, default);

        //Assert

        await integrationEventService.Received().AddAndSaveEventAsync(Arg.Any<OrderStatusChangedToAwaitingValidationIntegrationEvent>(), default);
    }
}
