using System.ComponentModel.DataAnnotations;
using eShop.Shared.Data;

namespace eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

public class Buyer : Entity, IAggregateRoot
{
    [Required]
    public string? IdentityGuid { get; private set; }

    public string? Name { get; private set; }

    private readonly List<PaymentMethod> _paymentMethods;

    public IEnumerable<PaymentMethod> PaymentMethods => this._paymentMethods.AsReadOnly();

    protected Buyer()
    {

        this._paymentMethods = [];
    }

    public Buyer(string identity, string name) : this()
    {
        this.IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
        this.Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
    }

    public PaymentMethod VerifyOrAddPaymentMethod(
        CardType cardType, string alias, string cardNumber,
        string securityNumber, string cardHolderName, DateTime expiration, int orderId)
    {
        var existingPayment = this._paymentMethods
            .SingleOrDefault(p => p.IsEqualTo(cardType, cardNumber, expiration));

        if (existingPayment is not null)
        {
            this.AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));

            return existingPayment;
        }

        var payment = new PaymentMethod(cardType, alias, cardNumber, securityNumber, cardHolderName, expiration);

        this._paymentMethods.Add(payment);

        this.AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId));

        return payment;
    }
}
