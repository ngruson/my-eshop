using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;

namespace eShop.Ordering.API.Application.DomainEventHandlers;

public class OrderShippedDomainEventHandler(
    IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
    ILogger<OrderShippedDomainEventHandler> logger,
    IRepository<Buyer> buyerRepository,
    IIntegrationEventService integrationEventService)
        : INotificationHandler<OrderShippedDomainEvent>
{
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> _orderRepository = orderRepository;
    private readonly IRepository<Buyer> _buyerRepository = buyerRepository;
    private readonly IIntegrationEventService _integrationEventService = integrationEventService;
    private readonly ILogger _logger = logger;

    public async Task Handle(OrderShippedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(this._logger, domainEvent.Order.Id, OrderStatus.Shipped);

        var order = await this._orderRepository.GetByIdAsync(domainEvent.Order.Id, cancellationToken);
        var buyer = await this._buyerRepository.GetByIdAsync(order!.BuyerId!.Value, cancellationToken);

        var integrationEvent = new OrderStatusChangedToShippedIntegrationEvent(order.Id, order.OrderStatus, buyer!.Name!, buyer.IdentityGuid!);
        await this._integrationEventService.AddAndSaveEventAsync(integrationEvent, cancellationToken);
    }
}
