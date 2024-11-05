using eShop.Ordering.API.Application.DomainEventHandlers;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.UnitTests.Application.DomainEventHandlers;
public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task BuyerIdAndPaymentIdUpdated(
        UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler sut,
        CardType cardType,
        string alias,
        string cardNumber,
        string securityNumber,
        string cardHolderName,
        Order order,
        Buyer buyer)
    {
        // Arrange

        BuyerAndPaymentMethodVerifiedDomainEvent evt = new(
            buyer,
            new PaymentMethod(cardType, alias, cardNumber, securityNumber, cardHolderName, DateTime.Now.AddYears(1)),
            order);

        order.SetPaymentMethodVerified(999, 999);

        //Act

        await sut.Handle(evt, default);

        //Assert

        Assert.Equal(evt.Buyer.Id, order.BuyerId);
        Assert.Equal(evt.Payment.Id, order.PaymentId);
    }
}
