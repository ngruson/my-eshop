using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;

namespace eShop.Ordering.API.Application.DomainEventHandlers;

public class OrderStatusChangedToStockConfirmedDomainEventHandler(
    IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
    IRepository<Buyer> buyerRepository,
    ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> logger,
    IIntegrationEventService integrationEventService)
        : INotificationHandler<OrderStatusChangedToStockConfirmedDomainEvent>
{
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> _orderRepository = orderRepository;
    private readonly IRepository<Buyer> _buyerRepository = buyerRepository;
    private readonly ILogger _logger = logger;
    private readonly IIntegrationEventService _integrationEventService = integrationEventService;

    public async Task Handle(OrderStatusChangedToStockConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(this._logger, domainEvent.OrderId, OrderStatus.StockConfirmed);

        Domain.AggregatesModel.OrderAggregate.Order? order =
            await this._orderRepository.SingleOrDefaultAsync(new GetOrderSpecification(domainEvent.OrderId), cancellationToken);

        Buyer? buyer = null;
        if (order!.BuyerId.HasValue)
        {
            buyer = await this._buyerRepository.GetByIdAsync(order!.BuyerId!.Value, cancellationToken);
        }

        OrderStatusChangedToStockConfirmedIntegrationEvent integrationEvent = new(order.ObjectId, order.OrderStatus, buyer?.Name, buyer?.IdentityGuid);
        await this._integrationEventService.AddAndSaveEventAsync(integrationEvent, cancellationToken);
    }
}
