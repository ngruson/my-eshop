namespace eShop.WebApp.Services;

public class BasketItem
{
    public required string Id { get; set; }
    public Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal OldUnitPrice { get; set; }
    public int Quantity { get; set; }
    public required string PictureUrl { get; set; }
}
