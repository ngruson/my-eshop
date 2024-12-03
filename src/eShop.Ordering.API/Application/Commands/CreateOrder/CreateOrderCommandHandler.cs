namespace eShop.Ordering.API.Application.Commands.CreateOrder;

using Ardalis.Result;
using eShop.Ordering.API.Application.Commands;
using eShop.Ordering.API.Application.Specifications;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Ordering.Domain.AggregatesModel.SalesTaxRateAggregate;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;

public class CreateOrderCommandHandler(
    IIntegrationEventService integrationEventService,
    IRepository<Order> orderRepository,
    IRepository<CardType> cardTypeRepository,
    IRepository<SalesTaxRate> salesTaxRateRepository,
    ILogger<CreateOrderCommandHandler> logger)
        : IRequestHandler<CreateOrderCommand, Result>
{
    private readonly IRepository<Order> orderRepository = orderRepository;
    private readonly IRepository<CardType> cardTypeRepository = cardTypeRepository;
    private readonly IRepository<SalesTaxRate> salesTaxRateRepository = salesTaxRateRepository;
    private readonly IIntegrationEventService orderingIntegrationEventService = integrationEventService;
    private readonly ILogger<CreateOrderCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Add Integration event to clean the basket
            OrderStartedIntegrationEvent orderStartedIntegrationEvent = new(request.UserId);
            await this.orderingIntegrationEventService.AddAndSaveEventAsync(orderStartedIntegrationEvent, cancellationToken);

            string maskedCCNumber = request.CardNumber[^4..].PadLeft(request.CardNumber.Length, 'X');
            CardType cardType = await this.cardTypeRepository.SingleOrDefaultAsync(new CardTypeSpecification(request.CardType), cancellationToken)
                ?? throw new KeyNotFoundException($"Card Type {request.CardType} not found.");
            Address address = new(request.Street!, request.City!, request.State!, request.Country!, request.ZipCode!);
            Order order = new(request.UserId, request.UserName!, request.BuyerName, address,
                cardType, maskedCCNumber, request.CardSecurityNumber!, request.CardHolderName!, request.CardExpiration);

            SalesTaxRate? salesTaxRate = null;
            if (address.State is not null)
            {
                SalesTaxRateSpecification salesTaxRateSpecification = new(address.State);
                salesTaxRate = await this.salesTaxRateRepository.SingleOrDefaultAsync(salesTaxRateSpecification, cancellationToken);
            }

            foreach (OrderItemDto item in request.Items)
            {
                order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, salesTaxRate?.Rate ?? 0, item.Discount, item.PictureUrl, item.Units);
            }

            this._logger.LogInformation("Creating Order - Order: {@Order}", order);

            await this.orderRepository.AddAsync(order, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to create order.";
            logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
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
