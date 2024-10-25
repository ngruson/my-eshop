namespace eShop.Ordering.Contracts.CreateOrder;

public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    decimal Discount,
    int Units,
    string PictureUrl);
