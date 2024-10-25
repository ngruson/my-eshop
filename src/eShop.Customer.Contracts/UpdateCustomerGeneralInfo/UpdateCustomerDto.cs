namespace eShop.Customer.Contracts.UpdateCustomerGeneralInfo;

public record UpdateCustomerDto(
    string UserName,
    string FirstName,
    string LastName,
    string Street,
    string City,
    string? State,
    string Country,
    string ZipCode);
