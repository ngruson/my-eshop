using System.ComponentModel.DataAnnotations;
using eShop.Shared.Data;

namespace eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

public class Buyer : Entity, IAggregateRoot
{
    [Required]
    public Guid IdentityGuid { get; private set; }

    public string? UserName { get; private set; }
    public string? Name { get; private set; }

    private readonly List<PaymentMethod> _paymentMethods;

    public IEnumerable<PaymentMethod> PaymentMethods => this._paymentMethods.AsReadOnly();

    protected Buyer()
    {

        this._paymentMethods = [];
    }

    public Buyer(Guid identity, string userName, string name) : this()
    {
        this.ObjectId = Guid.NewGuid();
        this.IdentityGuid = identity != Guid.Empty ? identity : throw new ArgumentNullException(nameof(identity));
        this.UserName = userName;
        this.Name = name;
    }

    public PaymentMethod VerifyOrAddPaymentMethod(
        CardType cardType, string alias, string cardNumber,
        string securityNumber, string cardHolderName, DateTime expiration, Order order)
    {
        PaymentMethod? existingPayment = this._paymentMethods
            .SingleOrDefault(p => p.IsEqualTo(cardType, cardNumber, expiration));

        if (existingPayment is not null)
        {
            this.AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, order));

            return existingPayment;
        }

        PaymentMethod payment = new(cardType, alias, cardNumber, securityNumber, cardHolderName, expiration);

        this._paymentMethods.Add(payment);

        this.AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, payment, order));

        return payment;
    }
}
