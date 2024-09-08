using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.DomainEventHandlers;

public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(
    IRepository<Domain.AggregatesModel.OrderAggregate.Order> orderRepository,
    ILogger<UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler> logger) : INotificationHandler<BuyerAndPaymentMethodVerifiedDomainEvent>
{
    private readonly IRepository<Domain.AggregatesModel.OrderAggregate.Order> _orderRepository = orderRepository;
    private readonly ILogger _logger = logger;

    // Domain Logic comment:
    // When the Buyer and Buyer's payment method have been created or verified that they existed, 
    // then we can update the original Order with the BuyerId and PaymentId (foreign keys)
    public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var orderToUpdate = await this._orderRepository.GetByIdAsync(domainEvent.OrderId, cancellationToken);
        orderToUpdate!.SetPaymentMethodVerified(domainEvent.Buyer.Id, domainEvent.Payment.Id); 
        OrderingApiTrace.LogOrderPaymentMethodUpdated(this._logger, domainEvent.OrderId, nameof(domainEvent.Payment), domainEvent.Payment.Id);
    }
}
