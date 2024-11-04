namespace eShop.Ordering.Contracts.UpdateOrder;

public record AddressDto(
    string City,
    string Country,
    string? State,
    string Street,
    string ZipCode
);
