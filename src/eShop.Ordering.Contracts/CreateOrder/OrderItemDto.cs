namespace eShop.Ordering.Contracts.CreateOrder;

public record OrderItemDto(
    int ProductId,
    string ProductName,
    decimal UnitPrice,
    decimal Discount,
    int Units,
    string PictureUrl);
