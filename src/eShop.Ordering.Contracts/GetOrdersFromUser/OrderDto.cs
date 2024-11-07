namespace eShop.Ordering.Contracts.GetOrdersFromUser;

public record OrderDto(
    Guid ObjectId,
    string OrderNumber,
    DateTime OrderDate,
    string BuyerName,
    string OrderStatus,
    decimal Total
);
