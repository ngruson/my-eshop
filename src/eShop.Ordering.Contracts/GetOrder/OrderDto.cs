namespace eShop.Ordering.Contracts.GetOrder;

public record OrderDto(
    Guid ObjectId,
    int OrderNumber,
    DateTime OrderDate,
    string? BuyerName,
    string OrderStatus,
    AddressDto Address,
    decimal NetTotal,
    decimal Total,
    OrderItemDto[] OrderItems,
    SalesTaxGroup[] SalesTaxGroups
);
