namespace eShop.Ordering.Contracts.GetOrder;

public record AddressDto(
    string City,
    string Country,
    string? State,
    string Street,
    string ZipCode
);
