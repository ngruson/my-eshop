namespace eShop.Ordering.API.Application.Commands.CreateOrder;

using Ardalis.Result;
using eShop.Ordering.API.Application.Commands;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;

// Regular CommandHandler
public class CreateOrderCommandHandler(
    IIntegrationEventService integrationEventService,
    IRepository<Order> orderRepository,
    IRepository<CardType> cardTypeRepository,
    ILogger<CreateOrderCommandHandler> logger)
        : IRequestHandler<CreateOrderCommand, Result>
{
    private readonly IRepository<Order> _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly IRepository<CardType> _cardTypeRepository = cardTypeRepository ?? throw new ArgumentNullException(nameof(cardTypeRepository));
    private readonly IIntegrationEventService _orderingIntegrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
    private readonly ILogger<CreateOrderCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Add Integration event to clean the basket
        OrderStartedIntegrationEvent orderStartedIntegrationEvent = new(request.UserId);
        await this._orderingIntegrationEventService.AddAndSaveEventAsync(orderStartedIntegrationEvent, cancellationToken);

        string maskedCCNumber = request.CardNumber[^4..].PadLeft(request.CardNumber.Length, 'X');
        CardType cardType = await this._cardTypeRepository.SingleOrDefaultAsync(new CardTypeSpecification(request.CardType), cancellationToken)
            ?? throw new KeyNotFoundException($"Card Type {request.CardType} not found.");
        Address address = new(request.Street!, request.City!, request.State!, request.Country!, request.ZipCode!);
        Order order = new(request.UserId, request.UserName!, address,
            cardType, maskedCCNumber, request.CardSecurityNumber!, request.CardHolderName!, request.CardExpiration);

        foreach (OrderItemDto item in request.Items)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
        }

        this._logger.LogInformation("Creating Order - Order: {@Order}", order);

        await this._orderRepository.AddAsync(order, cancellationToken);

        return Result.Success();
    }
}

// Use for Idempotency in Command process
public class CreateOrderIdentifiedCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<CreateOrderCommand, Result>> logger) : IdentifiedCommandHandler<CreateOrderCommand, Result>(mediator, requestManager, logger)
{
    protected override Result CreateResultForDuplicateRequest()
    {
        return Result.Success(); // Ignore duplicate requests for creating order.
    }
}
