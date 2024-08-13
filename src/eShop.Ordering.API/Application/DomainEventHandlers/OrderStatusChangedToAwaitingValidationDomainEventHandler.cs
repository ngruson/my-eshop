using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;

namespace eShop.Ordering.API.Application.DomainEventHandlers;

public class OrderStatusChangedToAwaitingValidationDomainEventHandler(
    IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
    ILogger<OrderStatusChangedToAwaitingValidationDomainEventHandler> logger,
    IRepository<Buyer> buyerRepository,
    IIntegrationEventService integrationEventService)
        : INotificationHandler<OrderStatusChangedToAwaitingValidationDomainEvent>
{
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> _orderRepository = orderRepository;
    private readonly ILogger _logger = logger;
    private readonly IRepository<Buyer> _buyerRepository = buyerRepository;
    private readonly IIntegrationEventService _integrationEventService = integrationEventService;

    public async Task Handle(OrderStatusChangedToAwaitingValidationDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(this._logger, domainEvent.OrderId, OrderStatus.AwaitingValidation);

        var order = await this._orderRepository.GetByIdAsync(domainEvent.OrderId, cancellationToken);
        var buyer = await this._buyerRepository.GetByIdAsync(order!.BuyerId!.Value, cancellationToken);

        var orderStockList = domainEvent.OrderItems
            .Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units));

        var integrationEvent = new OrderStatusChangedToAwaitingValidationIntegrationEvent(order.Id, order.OrderStatus, buyer!.Name!, buyer.IdentityGuid!, orderStockList);
        await this._integrationEventService.AddAndSaveEventAsync(integrationEvent, cancellationToken);
    }
}
