namespace eShop.Ordering.API.Application.Commands;

using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;

// Regular CommandHandler
public class CreateOrderCommandHandler(
    IIntegrationEventService integrationEventService,
    IRepository<Order> orderRepository,
    ILogger<CreateOrderCommandHandler> logger)
        : IRequestHandler<CreateOrderCommand, bool>
{
    private readonly IRepository<Order> _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly IIntegrationEventService _orderingIntegrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
    private readonly ILogger<CreateOrderCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<bool> Handle(CreateOrderCommand message, CancellationToken cancellationToken)
    {
        // Add Integration event to clean the basket
        var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(message.UserId!);
        await this._orderingIntegrationEventService.AddAndSaveEventAsync(orderStartedIntegrationEvent, cancellationToken);

        // Add/Update the Buyer AggregateRoot
        // DDD patterns comment: Add child entities and value-objects through the Order Aggregate-Root
        // methods and constructor so validations, invariants and business logic 
        // make sure that consistency is preserved across the whole aggregate
        var address = new Address(message.Street!, message.City!, message.State!, message.Country!, message.ZipCode!);
        var order = new Order(message.UserId!, message.UserName!, address,
            message.CardTypeId, message.CardNumber!, message.CardSecurityNumber!, message.CardHolderName!, message.CardExpiration);

        foreach (var item in message.OrderItems)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
        }

        this._logger.LogInformation("Creating Order - Order: {@Order}", order);

        await this._orderRepository.AddAsync(order, cancellationToken);

        return true;
    }
}

// Use for Idempotency in Command process
public class CreateOrderIdentifiedCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>> logger) : IdentifiedCommandHandler<CreateOrderCommand, bool>(mediator, requestManager, logger)
{
    protected override bool CreateResultForDuplicateRequest()
    {
        return true; // Ignore duplicate requests for creating order.
    }
}
