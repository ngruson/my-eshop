namespace eShop.Ordering.Contracts.GetOrder;

public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    decimal Discount,
    int Units,
    decimal Total,
    string PictureUrl);