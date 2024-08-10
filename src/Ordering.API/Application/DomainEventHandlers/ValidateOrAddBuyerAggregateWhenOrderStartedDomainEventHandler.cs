using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;
using eShop.Shared.IntegrationEvents;

namespace eShop.Ordering.API.Application.DomainEventHandlers;

public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(
    ILogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler> logger,
    IRepository<Buyer> buyerRepository,
    IIntegrationEventService integrationEventService)
        : INotificationHandler<OrderStartedDomainEvent>
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IRepository<Buyer> _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
    private readonly IIntegrationEventService _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));

    public async Task Handle(OrderStartedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var cardTypeId = domainEvent.CardTypeId != 0 ? domainEvent.CardTypeId : 1;
        
        var buyer = await this._buyerRepository.SingleOrDefaultAsync(
            new GetBuyerByIdentitySpecification(domainEvent.UserId),
            cancellationToken);

        var buyerExisted = buyer is not null;

        if (!buyerExisted)
        {
            buyer = new Buyer(domainEvent.UserId, domainEvent.UserName);
        }

        // REVIEW: The event this creates needs to be sent after SaveChanges has propagated the buyer Id. It currently only
        // works by coincidence. If we remove HiLo or if anything decides to yield earlier, it will break.

        buyer!.VerifyOrAddPaymentMethod(cardTypeId,
            $"Payment Method on {DateTime.UtcNow}",
            domainEvent.CardNumber,
            domainEvent.CardSecurityNumber,
            domainEvent.CardHolderName,
            domainEvent.CardExpiration,
            domainEvent.Order.Id);

        if (!buyerExisted)
        {
            await this._buyerRepository.AddAsync(buyer, cancellationToken);
        }

        await this._buyerRepository.UnitOfWork
            .SaveEntitiesAsync(cancellationToken);

        var integrationEvent = new OrderStatusChangedToSubmittedIntegrationEvent(domainEvent.Order.Id, domainEvent.Order.OrderStatus, buyer.Name!, buyer.IdentityGuid!);
        await this._integrationEventService.AddAndSaveEventAsync(integrationEvent, cancellationToken);
        OrderingApiTrace.LogOrderBuyerAndPaymentValidatedOrUpdated(_logger, buyer.Id, domainEvent.Order.Id);
    }
}
