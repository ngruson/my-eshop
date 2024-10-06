using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Queries;

public record OrderItem
{
    public string? ProductName { get; init; }
    public int Units { get; init; }
    public double UnitPrice { get; init; }
    public string? PictureUrl { get; init; }
}

public record Order : IAggregateRoot
{
    public int OrderNumber { get; init; }
    public DateTime Date { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? Street { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? ZipCode { get; init; }
    public string? Country { get; init; }
    public List<OrderItem>? OrderItems { get; set; }
    public decimal Total { get; set; }
}

public record OrderSummary
{
    public int OrderNumber { get; init; }
    public DateTime Date { get; init; }
    public string? Status { get; init; }
    public double Total { get; init; }
}
