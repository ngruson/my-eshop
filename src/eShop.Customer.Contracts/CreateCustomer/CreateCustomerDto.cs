namespace eShop.Customer.Contracts.CreateCustomer;

public record CreateCustomerDto(
    string FirstName,
    string LastName,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    string? CardNumber,
    string? SecurityNumber,
    string? Expiration,
    string? CardHolderName,
    int? CardType);
