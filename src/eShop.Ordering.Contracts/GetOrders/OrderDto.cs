namespace eShop.Ordering.Contracts.GetOrders;

public record OrderDto(
    string OrderNumber,
    DateTime OrderDate,
    string BuyerName,
    string OrderStatus,
    decimal Total
);
