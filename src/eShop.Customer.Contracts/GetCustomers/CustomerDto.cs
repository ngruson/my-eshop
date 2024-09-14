using System.ComponentModel.DataAnnotations;

namespace eShop.Customer.Contracts.GetCustomers;

public record CustomerDto(
    string FirstName,
    string LastName,
    string? CardNumber,
    string? SecurityNumber,
    string? Expiration,
    string? CardHolderName,
    int CardType,
    string? Street,
    string? City,
    string? State,
    string? Country,
    string? ZipCode
);
