using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;
using eShop.Shared.Features;
using eShop.Shared.IntegrationEvents;
using Microsoft.Extensions.Options;

namespace eShop.Ordering.API.Application.DomainEventHandlers;

public class OrderStatusChangedToStockConfirmedDomainEventHandler(
    IRepository<Order> orderRepository,
    IRepository<Buyer> buyerRepository,
    ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> logger,
    IIntegrationEventService integrationEventService,
    IOptions<FeaturesConfiguration> features)
        : INotificationHandler<OrderStatusChangedToStockConfirmedDomainEvent>
{
    public async Task Handle(OrderStatusChangedToStockConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(logger, domainEvent.OrderId, OrderStatus.StockConfirmed);

        Order? order = await orderRepository.SingleOrDefaultAsync(new GetOrderSpecification(domainEvent.OrderId), cancellationToken);

        Buyer? buyer = null;
        if (order!.BuyerId.HasValue)
        {
            buyer = await buyerRepository.GetByIdAsync(order!.BuyerId!.Value, cancellationToken);
        }

        if (features.Value.Workflow.Enabled is false)
        {
            OrderStatusChangedToStockConfirmedIntegrationEvent integrationEvent = new(order.ObjectId, order.OrderStatus, buyer?.Name, buyer?.IdentityGuid);
            await integrationEventService.AddAndSaveEventAsync(integrationEvent, cancellationToken);
        }
    }
}
