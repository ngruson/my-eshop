using System.ComponentModel.DataAnnotations;

namespace eShop.AdminApp.Application.Queries.GetCustomers;

public class CustomerViewModel(string firstName, string lastName, string street, string city, string state, string country, string zipCode,
    string cardNumber, string securityNumber, string expiration, string cardHolderName, int cardType)
{
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = firstName;

    [Display(Name = "Last Name")]
    public string LastName { get; set; } = lastName;

    public string Street { get; set; } = street;
    public string City { get; set; } = city;
    public string State { get; set; } = state;
    public string Country { get; set; } = country;

    [Display(Name = "Zip code")]
    public string ZipCode { get; set; } = zipCode;
    
    public string CardNumber { get; set; } = cardNumber;
    public string SecurityNumber { get; set; } = securityNumber;
    public string Expiration { get; set; } = expiration;
    public string CardHolderName { get; set; } = cardHolderName;
    public int CardType { get; set; } = cardType;

    public string FullName => $"{this.FirstName} {this.LastName}";
}
