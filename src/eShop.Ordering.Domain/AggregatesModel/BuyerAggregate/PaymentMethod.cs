using System.ComponentModel.DataAnnotations;
using eShop.Shared.Data;

namespace eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

public class PaymentMethod : Entity
{
    [Required]
    private readonly string? _alias;
    [Required]
    private readonly string? _cardNumber;
    private readonly string? _securityNumber;
    [Required]
    private readonly string? _cardHolderName;
    private readonly DateTime _expiration;

    private readonly CardType? _cardType;
    public CardType? CardType { get; private set; }

    public string? Alias => this._alias;
    public string? SecurityNumber => this._securityNumber;
    public string? CardHolderName => this._cardHolderName;

    protected PaymentMethod() { }

    public PaymentMethod(CardType cardType, string alias, string cardNumber, string securityNumber, string cardHolderName, DateTime expiration)
    {
        this._cardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : throw new OrderingDomainException(nameof(cardNumber));
        this._securityNumber = !string.IsNullOrWhiteSpace(securityNumber) ? securityNumber : throw new OrderingDomainException(nameof(securityNumber));
        this._cardHolderName = !string.IsNullOrWhiteSpace(cardHolderName) ? cardHolderName : throw new OrderingDomainException(nameof(cardHolderName));

        if (expiration < DateTime.UtcNow)
        {
            throw new OrderingDomainException(nameof(expiration));
        }

        this._alias = alias;
        this._expiration = expiration;
        this._cardType = cardType;
    }

    public bool IsEqualTo(CardType cardType, string cardNumber, DateTime expiration)
    {
        return this._cardType == cardType
            && this._cardNumber == cardNumber
            && this._expiration == expiration;
    }
}
