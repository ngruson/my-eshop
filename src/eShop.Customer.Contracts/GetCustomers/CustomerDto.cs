namespace eShop.Customer.Contracts.GetCustomers;

public record CustomerDto(
    Guid ObjectId,
    string UserName,
    CustomerNameDto Name,
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
