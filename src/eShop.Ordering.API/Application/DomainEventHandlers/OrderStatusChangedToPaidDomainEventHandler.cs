using eShop.Ordering.API.Application.Specifications;
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

        Domain.AggregatesModel.OrderAggregate.Order? order =
            await this._orderRepository.SingleOrDefaultAsync(new GetOrderSpecification(domainEvent.OrderId), cancellationToken);

        Buyer? buyer = null;
        if (order!.BuyerId.HasValue)
        {
            buyer = await this._buyerRepository.GetByIdAsync(order!.BuyerId!.Value, cancellationToken);
        }

        OrderStockItem[] orderStockList = domainEvent.OrderItems
            .Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units))
            .ToArray();

        OrderStatusChangedToPaidIntegrationEvent integrationEvent = new(
            domainEvent.OrderId,
            order.OrderStatus,
            buyer!.Name!,
            buyer.IdentityGuid,
            orderStockList);

        await this._orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent, cancellationToken);
    }
}
