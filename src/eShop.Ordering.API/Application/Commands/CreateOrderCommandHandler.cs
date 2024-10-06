namespace eShop.Ordering.API.Application.Commands;

using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;

// Regular CommandHandler
public class CreateOrderCommandHandler(
    IIntegrationEventService integrationEventService,
    IRepository<Order> orderRepository,
    IRepository<CardType> cardTypeRepository,
    ILogger<CreateOrderCommandHandler> logger)
        : IRequestHandler<CreateOrderCommand, bool>
{
    private readonly IRepository<Order> _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly IRepository<CardType> _cardTypeRepository = cardTypeRepository ?? throw new ArgumentNullException(nameof(cardTypeRepository));
    private readonly IIntegrationEventService _orderingIntegrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
    private readonly ILogger<CreateOrderCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<bool> Handle(CreateOrderCommand message, CancellationToken cancellationToken)
    {
        // Add Integration event to clean the basket
        var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(message.UserId!);
        await this._orderingIntegrationEventService.AddAndSaveEventAsync(orderStartedIntegrationEvent, cancellationToken);

        CardType cardType = await this._cardTypeRepository.SingleOrDefaultAsync(new CardTypeSpecification(message.CardType), cancellationToken)
            ?? throw new KeyNotFoundException($"Card Type {message.CardType} not found.");
        Address address = new(message.Street!, message.City!, message.State!, message.Country!, message.ZipCode!);
        Order order = new(message.UserId!, message.UserName!, address,
            cardType, message.CardNumber!, message.CardSecurityNumber!, message.CardHolderName!, message.CardExpiration);

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
