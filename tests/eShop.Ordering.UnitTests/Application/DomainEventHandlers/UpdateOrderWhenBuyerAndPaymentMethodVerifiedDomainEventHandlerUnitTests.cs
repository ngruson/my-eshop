using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.API.Application.DomainEventHandlers;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Shared.Data;

namespace Ordering.UnitTests.Application.DomainEventHandlers;
public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandlerUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task BuyerIdAndPaymentIdUpdated(
        [Substitute, Frozen] IRepository<Order> orderRepository,
        [Substitute, Frozen] IRepository<Buyer> buyerRepository,
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
            order.Id
        );

        order.SetPaymentMethodVerified(999, 999);

        orderRepository.GetByIdAsync(evt.OrderId, default)
            .Returns(order);

        buyerRepository.GetByIdAsync(order.BuyerId.Value, default)
            .Returns(buyer);

        //Act

        await sut.Handle(evt, default);

        //Assert

        Assert.Equal(evt.Buyer.Id, order.BuyerId);
        Assert.Equal(evt.Payment.Id, order.PaymentId);
    }
}
