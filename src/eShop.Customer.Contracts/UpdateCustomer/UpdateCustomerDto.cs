namespace eShop.Customer.Contracts.UpdateCustomer;

public record UpdateCustomerDto(
    Guid ObjectId,
    string UserName,
    string FirstName,
    string LastName,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode);
