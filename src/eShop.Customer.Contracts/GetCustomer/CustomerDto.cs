namespace eShop.Customer.Contracts.GetCustomer;

public record CustomerDto(
    string FirstName,
    string LastName,
    string CardNumber,
    string SecurityNumber,
    string Expiration,
    string CardHolderName,
    int CardType,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode
);
