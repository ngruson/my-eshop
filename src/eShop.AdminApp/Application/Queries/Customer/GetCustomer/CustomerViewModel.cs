using System.ComponentModel.DataAnnotations;

namespace eShop.AdminApp.Application.Queries.Customer.GetCustomer;

public class CustomerViewModel(Guid objectId, string userName, string firstName, string lastName, string street, string city, string state, string country, string zipCode,
    string? cardNumber, string? expiration, string? cardHolderName, string? cardType)
{
    public CustomerViewModel() : this(Guid.Empty, "", "", "", "", "", "", "US", "", "", "", "", "") { }

    public Guid ObjectId { get; set; } = objectId;

    [Display(Name = "Username"), Required]
    public string UserName { get; set; } = userName;

    [Display(Name = "First Name"), Required]
    public string FirstName { get; set; } = firstName;

    [Display(Name = "Last Name"), Required]
    public string LastName { get; set; } = lastName;

    [Required]
    public string Street { get; set; } = street;

    [Required]
    public string City { get; set; } = city;

    [Required]
    public string State { get; set; } = state;

    [Required]
    public string Country { get; set; } = country;

    [Display(Name = "Zip code"), Required]
    public string ZipCode { get; set; } = zipCode;

    public string? CardNumber { get; set; } = cardNumber;
    public string? Expiration { get; set; } = expiration;
    public string? CardHolderName { get; set; } = cardHolderName;
    public string? CardType { get; set; } = cardType;

    public string FullName => $"{this.FirstName} {this.LastName}";
    public bool NewCustomer { get; set; }
}
