namespace eShop.Ordering.Contracts.GetOrders;

public record OrderDto(
    Guid ObjectId,
    string OrderNumber,
    DateTime OrderDate,
    string BuyerName,
    string OrderStatus,
    decimal Total
);
