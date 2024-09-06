using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;
using Order = eShop.Ordering.Domain.AggregatesModel.OrderAggregate.Order;

namespace eShop.Ordering.API.Application.DomainEventHandlers;

public partial class OrderCancelledDomainEventHandler(
    IRepository<Order> orderRepository,
    ILogger<OrderCancelledDomainEventHandler> logger,
    IRepository<Buyer> buyerRepository,
    IIntegrationEventService integrationEventService)
        : INotificationHandler<OrderCancelledDomainEvent>
{
    private readonly IRepository<Order> _orderRepository = orderRepository;
    private readonly IRepository<Buyer> _buyerRepository = buyerRepository;
    private readonly ILogger _logger = logger;
    private readonly IIntegrationEventService _integrationEventService = integrationEventService;

    public async Task Handle(OrderCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(this._logger, domainEvent.Order.Id, OrderStatus.Cancelled);

        var order = await this._orderRepository.GetByIdAsync(domainEvent.Order.Id, cancellationToken);
        var buyer = await this._buyerRepository.GetByIdAsync(order!.BuyerId!.Value, cancellationToken);

        var integrationEvent = new OrderStatusChangedToCancelledIntegrationEvent(order.Id, order.OrderStatus, buyer!.Name!, buyer.IdentityGuid!);
        await this._integrationEventService.AddAndSaveEventAsync(integrationEvent, cancellationToken);
    }
}
