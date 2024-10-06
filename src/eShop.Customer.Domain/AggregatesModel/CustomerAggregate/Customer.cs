using System.ComponentModel.DataAnnotations;
using eShop.Shared.Data;

namespace eShop.Customer.Domain.AggregatesModel.CustomerAggregate;

public class Customer : Entity, IAggregateRoot
{
    public Customer(Guid objectId, string? userName, string? firstName, string? lastName,
        string? street, string? city, string? state, string? country, string? zipCode,
        string? cardNumber, string? securityNumber, string? expiration, string? cardHolderName, CardType? cardType)
    {
        this.ObjectId = objectId;
        this.UserName = userName;
        this.FirstName = firstName;
        this.LastName = lastName;        
        this.Street = street;
        this.City = city;
        this.State = state;
        this.Country = country;
        this.ZipCode = zipCode;
        this.CardNumber = cardNumber;
        this.SecurityNumber = securityNumber;
        this.Expiration = expiration;
        this.CardHolderName = cardHolderName;
        this.CardType = cardType;
    }

    private Customer() { }

    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
    public string? CardNumber { get; set; }
    public string? SecurityNumber { get; set; }
    [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Expiration should match a valid MM/YY value")]
    public string? Expiration { get; set; }
    public string? CardHolderName { get; set; }
    public CardType? CardType { get; set; }
}
