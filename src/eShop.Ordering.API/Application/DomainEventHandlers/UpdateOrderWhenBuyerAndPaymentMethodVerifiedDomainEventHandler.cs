namespace eShop.Ordering.API.Application.DomainEventHandlers;

public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(
    ILogger<UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler> logger) : INotificationHandler<BuyerAndPaymentMethodVerifiedDomainEvent>
{
    private readonly ILogger _logger = logger;

    // Domain Logic comment:
    // When the Buyer and Buyer's payment method have been created or verified that they existed, 
    // then we can update the original Order with the BuyerId and PaymentId (foreign keys)
    public Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        Domain.AggregatesModel.OrderAggregate.Order order = domainEvent.Order;
        order.SetPaymentMethodVerified(domainEvent.Buyer.Id, domainEvent.Payment.Id);
        OrderingApiTrace.LogOrderPaymentMethodUpdated(this._logger, domainEvent.Order.ObjectId, nameof(domainEvent.Payment), domainEvent.Payment.Id);
        return Task.CompletedTask;
    }
}
