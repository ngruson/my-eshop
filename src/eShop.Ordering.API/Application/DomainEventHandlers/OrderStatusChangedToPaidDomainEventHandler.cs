using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;

namespace eShop.Ordering.API.Application.DomainEventHandlers;

public class OrderStatusChangedToPaidDomainEventHandler(
    IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
    ILogger<OrderStatusChangedToPaidDomainEventHandler> logger,
    IRepository<Buyer> buyerRepository,
    IIntegrationEventService integrationEventService) : INotificationHandler<OrderStatusChangedToPaidDomainEvent>
{
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> _orderRepository = orderRepository;
    private readonly ILogger _logger = logger;
    private readonly IRepository<Buyer> _buyerRepository = buyerRepository;
    private readonly IIntegrationEventService _orderingIntegrationEventService = integrationEventService;

    public async Task Handle(OrderStatusChangedToPaidDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(this._logger, domainEvent.OrderId, OrderStatus.Paid);

        var order = await this._orderRepository.GetByIdAsync(domainEvent.OrderId, cancellationToken);
        Buyer? buyer = null;
        if (order!.BuyerId.HasValue)
        {
            buyer = await this._buyerRepository.GetByIdAsync(order!.BuyerId!.Value, cancellationToken);
        }

        var orderStockList = domainEvent.OrderItems
            .Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units));

        var integrationEvent = new OrderStatusChangedToPaidIntegrationEvent(
            domainEvent.OrderId,
            order.OrderStatus,
            buyer?.Name,
            buyer?.IdentityGuid,
            orderStockList);

        await this._orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent, cancellationToken);
    }
}
