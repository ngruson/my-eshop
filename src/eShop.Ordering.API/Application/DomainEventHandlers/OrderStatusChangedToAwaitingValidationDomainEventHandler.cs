using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;
using eShop.Shared.Features;
using eShop.Shared.IntegrationEvents;
using Microsoft.Extensions.Options;

namespace eShop.Ordering.API.Application.DomainEventHandlers;

public class OrderStatusChangedToAwaitingValidationDomainEventHandler(
    IRepository<Order> orderRepository,
    ILogger<OrderStatusChangedToAwaitingValidationDomainEventHandler> logger,
    IRepository<Buyer> buyerRepository,
    IIntegrationEventService integrationEventService,
    IOptions<FeaturesConfiguration> features)
        : INotificationHandler<OrderStatusChangedToAwaitingValidationDomainEvent>
{
    private readonly IRepository<Order> _orderRepository = orderRepository;
    private readonly ILogger _logger = logger;
    private readonly IRepository<Buyer> _buyerRepository = buyerRepository;
    private readonly IIntegrationEventService _integrationEventService = integrationEventService;

    public async Task Handle(OrderStatusChangedToAwaitingValidationDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(this._logger, domainEvent.OrderId, OrderStatus.AwaitingValidation);

        Order? order = await this._orderRepository.SingleOrDefaultAsync(
            new GetOrderSpecification(domainEvent.OrderId), cancellationToken);

        Buyer? buyer = null;
        if (order!.BuyerId.HasValue)
        {
            buyer = await this._buyerRepository.GetByIdAsync(order!.BuyerId!.Value, cancellationToken);
        }

        if (features.Value.Workflow.Enabled is false)
        {
            OrderStatusChangedToAwaitingValidationIntegrationEvent integrationEvent = new(
                order.ObjectId, order.OrderStatus, buyer?.Name, buyer?.IdentityGuid,
                [.. domainEvent.OrderItems.Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units))],
                order.WorkflowInstanceId);

            await this._integrationEventService.AddAndSaveEventAsync(integrationEvent, cancellationToken);
        }
    }
}
