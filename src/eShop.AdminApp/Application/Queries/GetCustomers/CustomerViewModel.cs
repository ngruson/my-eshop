using System.ComponentModel.DataAnnotations;

namespace eShop.AdminApp.Application.Queries.GetCustomers;

public class CustomerViewModel(string firstName, string lastName, string? street, string? city, string? state, string? country, string? zipCode)
{
    [Display(Name = "First Name")]
    public string FirstName { get; private init; } = firstName;

    [Display(Name = "Last Name")]
    public string LastName { get; private init; } = lastName;

    public string? Street { get; private init; } = street;
    public string? City { get; private init; } = city;
    public string? State { get; private init; } = state;
    public string? Country { get; private init; } = country;

    [Display(Name = "Zip code")]
    public string? ZipCode { get; private init; } = zipCode;
}
