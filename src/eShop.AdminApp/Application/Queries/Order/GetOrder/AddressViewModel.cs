namespace eShop.AdminApp.Application.Queries.Order.GetOrder;

public class AddressViewModel
{
    public required string City { get; set; }
    public required string Country { get; set; }
    public string? State { get; set; }
    public required string Street { get; set; }
    public required string ZipCode { get; set; }
}
