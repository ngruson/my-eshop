namespace eShop.Ordering.API.Application.Queries.GetOrders;

internal record OrderDto(
    DateTime OrderDate,
    string BuyerName,
    string OrderStatus,
    decimal Total
);
