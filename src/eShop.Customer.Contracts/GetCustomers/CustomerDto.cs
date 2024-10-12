namespace eShop.Customer.Contracts.GetCustomers;

public record CustomerDto(
    Guid ObjectId,
    string UserName,
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
    string? CardType,
    bool IsDeleted);
